namespace Byndyusoft.ArchitectureTesting.DependencyValidators.Tests.Infrastructure.Logging
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMockLogger(this IServiceCollection services, Mock<ILogger> mockLogger)
        {
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            return services
                .AddSingleton(mockLoggerFactory.Object)
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        }
    }
}