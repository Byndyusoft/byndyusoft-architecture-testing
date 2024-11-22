namespace Byndyusoft.ArchitectureTesting.DependencyValidators.Tests.Infrastructure.Logging
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Moq;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNullLogger(this IServiceCollection services)
        {
            return services
                .AddSingleton<ILoggerFactory, NullLoggerFactory>()
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        }
    }
}