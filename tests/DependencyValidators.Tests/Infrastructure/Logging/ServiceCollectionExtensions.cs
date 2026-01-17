namespace Byndyusoft.ArchitectureTesting.DependencyValidators.Tests.Infrastructure.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNullLogger(this IServiceCollection services)
        => services
            .AddSingleton<ILoggerFactory, NullLoggerFactory>()
            .AddSingleton(typeof(ILogger<>), typeof(Logger<>));
}