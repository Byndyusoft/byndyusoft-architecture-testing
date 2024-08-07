namespace MusicalityLabs.Storage.Api.Tests.Validation.Extensions
{
    using System;
    using System.Collections.Generic;
    using Byndyusoft.ArchitectureTesting.Abstractions.ServiceImplementations;
    using Byndyusoft.ArchitectureTesting.Abstractions.Validation.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceImplementationExtensions
    {
        private static IServiceProvider BuildServiceProvider(Type startupType)
        {
            var services = new ServiceCollection();

            startupType.GetMethod("ConfigureServices")!.Invoke(
                Activator.CreateInstance(
                    startupType,
                    new ConfigurationBuilder()
                        .AddInMemoryCollection(new[] {new KeyValuePair<string, string>("ConnectionStrings:Main", "Server=")})
                        .Build()
                ),
                new object[] {services}
            );

            return services.BuildServiceProvider(true);
        }

        public static ServiceImplementation Create(Type startupType)
        {
            var rootAssembly = startupType.Assembly;
            var implementation
                = new ServiceImplementation
                  {
                      ServiceAssemblies = rootAssembly.GetServiceAssemblies(
                          x => x.FullNameStartsWith("Byndyusoft.", "MusicalityLabs.")
                      ),
                      ServiceProvider = BuildServiceProvider(startupType)
                  };

            return implementation;
        }
    }
}