namespace Byndyusoft.ArchitectureTesting.DependencyValidators.Tests.Infrastructure.Logging
{
    using Microsoft.Extensions.Logging;
    using Moq;

    public static class MockLoggerExtensions
    {
        public static Mock<ILogger> CreateMockLogger()
        {
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
            return mockLogger;
        }
    }
}