namespace Byndyusoft.ArchitectureTesting.ServiceImplementations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    internal class HostInitializer : IObserver<DiagnosticListener>, IObserver<KeyValuePair<string, object?>>
    {
        private readonly MethodInfo _entryPoint;
        private readonly Action<IHostBuilder> _hostBuilderConfigurator;

        private static readonly TimeSpan DefaultInitializationTimeout = SetupDefaultInitializationTimeout();
        private static readonly AsyncLocal<HostInitializer> CurrentListener = new();
        private readonly TaskCompletionSource<IHost> _hostInitializationTaskCompletionSource = new();

        private IDisposable? _hostInitializationEventsSubscription;

        public HostInitializer(MethodInfo entryPoint, Action<IHostBuilder> hostBuilderConfigurator)
        {
            _entryPoint = entryPoint;
            _hostBuilderConfigurator = hostBuilderConfigurator;
        }

        public IHost Initialize(TimeSpan? initializationTimeout)
        {
            using var listenersSubscription = DiagnosticListener.AllListeners.Subscribe(this);

            var thread = new Thread(StartEntryPoint) { IsBackground = true };
            thread.Start();

            var waitTimeout = initializationTimeout ?? DefaultInitializationTimeout;
            try
            {
                if (_hostInitializationTaskCompletionSource.Task.Wait(waitTimeout) == false)
                    throw new InvalidOperationException(
                        $"Timed out waiting for the entry point to build the {nameof(IHost)} after {waitTimeout.TotalSeconds} seconds"
                    );
            }
            catch (AggregateException)
            {
                if (_hostInitializationTaskCompletionSource.Task.IsCompleted == false)
                    throw;
            }

            return _hostInitializationTaskCompletionSource.Task.GetAwaiter().GetResult();
        }

        public void OnCompleted() => _hostInitializationEventsSubscription?.Dispose();

        public void OnError(Exception error)
        {
        }

        public void OnNext(DiagnosticListener value)
        {
            if (CurrentListener.Value != this)
                return;

            if (value.Name == "Microsoft.Extensions.Hosting")
                _hostInitializationEventsSubscription = value.Subscribe(this);
        }

        public void OnNext(KeyValuePair<string, object?> value)
        {
            if (CurrentListener.Value != this)
                return;

            switch (value.Key)
            {
                case "HostBuilding":
                    _hostBuilderConfigurator((IHostBuilder)value.Value!);

                    break;
                case "HostBuilt":
                    _hostInitializationTaskCompletionSource.TrySetResult((IHost)value.Value!);
                    ThrowHostAbortedException();

                    break;
            }
        }

        private static TimeSpan SetupDefaultInitializationTimeout()
            => Debugger.IsAttached
                   ? Timeout.InfiniteTimeSpan
                   : TimeSpan.FromMinutes(5);

        private void StartEntryPoint()
        {
            try
            {
                CurrentListener.Value = this;

                _entryPoint.Invoke(
                    null,
                    _entryPoint.GetParameters().Length == 0
                        ? Array.Empty<object>()
                        : new object[] { Array.Empty<string>() }
                );

                _hostInitializationTaskCompletionSource.TrySetException(
                    new InvalidOperationException($"The entry point exited without ever building an {nameof(IHost)}.")
                );
            }
            catch (TargetInvocationException targetInvocationException)
            {
                if (targetInvocationException.InnerException?.GetType().Name != nameof(HostAbortedException))
                    _hostInitializationTaskCompletionSource.TrySetException(
                        targetInvocationException.InnerException ?? targetInvocationException
                    );
            }
            catch (Exception exception)
            {
                _hostInitializationTaskCompletionSource.TrySetException(exception);
            }
        }

        private static void ThrowHostAbortedException()
        {
            var publicHostAbortedExceptionType = Type.GetType(
                $"Microsoft.Extensions.Hosting.{nameof(HostAbortedException)}, Microsoft.Extensions.Hosting.Abstractions",
                false
            );

            if (publicHostAbortedExceptionType != null)
                throw (Exception)Activator.CreateInstance(publicHostAbortedExceptionType)!;

            throw new HostAbortedException();
        }

        private sealed class HostAbortedException : Exception
        {
        }
    }
}