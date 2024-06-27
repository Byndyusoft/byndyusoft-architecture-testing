namespace Byndyusoft.ArchitectureTesting.Abstractions.ServiceImplementations
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Реализация проверяемого сервиса
    /// </summary>
    public class ServiceImplementation : IDisposable
    {
        /// <summary>
        /// Коллекция сборок, из которых состоит сервис
        /// </summary>
        public Assembly[] ServiceAssemblies { get; set; }

        /// <summary>
        /// Зависимости, из которых состоит сервис
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(ServiceProvider is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}