namespace Byndyusoft.ArchitectureTesting.ReferenceCompliance.ServiceImplementations
{
    using System;
    using System.Reflection;
    using Abstractions.ServiceImplementations;
    using Common.Abstractions.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public abstract class ServiceImplementationFactoryBase<TEntryPoint> where TEntryPoint : class
    {
        protected abstract bool IsImplementationAssembly(AssemblyName assemblyName);

        protected virtual void ConfigureImplementationComponents(
            HostBuilderContext hostBuilderContext,
            IServiceCollection serviceCollection
        )
        {
        }

        public ServiceImplementation Create(TimeSpan? creatingTimeout = null)
        {
            var rootAssembly = typeof(TEntryPoint).Assembly;
            var host = new HostInitializer(rootAssembly.EntryPoint, x => x.ConfigureServices(ConfigureImplementationComponents))
                .Initialize(creatingTimeout);

            return new ServiceImplementation
                   {
                       ServiceAssemblies = rootAssembly.GetServiceAssemblies(IsImplementationAssembly),
                       ServiceProvider = host.Services
                   };
        }
    }
}