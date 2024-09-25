namespace Byndyusoft.ArchitectureTesting.DependencyValidators.Tests
{
    using Abstractions.ServiceContracts.Dependencies;
    using FluentAssertions;
    using System;
    using Abstractions.ServiceImplementations;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using Fakes;
    using Byndyusoft.Net.Kafka.Abstractions.Producing;
    using Infrastructure.Logging;
    using Net.Kafka;
    using Net.Kafka.Abstractions.Consuming;
    using Net.Kafka.Configuration;
    using Validation.KafkaDependencyValidators;

    public class KafkaDependencyValidatorTests
    {
        public static readonly TheoryData<DependencyValidatorTestCase> KafkaDependenciesValidationTestCases;

        private static ServiceImplementation CreateServiceImplementation(Action<IServiceCollection>? configureDependencies = null)
        {
            var services = new ServiceCollection();
            configureDependencies?.Invoke(services);

            return new ServiceImplementation {ServiceProvider = services.BuildServiceProvider()};
        }

        static KafkaDependencyValidatorTests()
        {
            const string kafkaTopicName = "composer_assistant.entity.creation";
            KafkaDependenciesValidationTestCases
                = new TheoryData<DependencyValidatorTestCase>
                  {
                      new DependencyValidatorTestCase
                      {
                          Description = "Kafka message producer from the architecture was not implemented in the service",
                          Dependencies
                              = new DependencyBase[]
                                {
                                    new KafkaDependency
                                    {
                                        Direction = MqDependencyDirection.Outgoing,
                                        Name = kafkaTopicName
                                    }
                                },
                          ServiceImplementation = CreateServiceImplementation(),
                          ExpectedErrors
                              = new[]
                                {
                                    $"Message producer for topic {kafkaTopicName} is missed"
                                }
                      },
                      new DependencyValidatorTestCase
                      {
                          Description = "Kafka message handler from the architecture was not implemented in the service",
                          Dependencies
                              = new DependencyBase[]
                                {
                                    new KafkaDependency
                                    {
                                        Direction = MqDependencyDirection.Incoming,
                                        Name = kafkaTopicName
                                    }
                                },
                          ServiceImplementation = CreateServiceImplementation(),
                          ExpectedErrors
                              = new[]
                                {
                                    $"Message handler for topic {kafkaTopicName} is missed"
                                }
                      },
                      new DependencyValidatorTestCase
                      {
                          Description = "Kafka message producer and handler missing from the architecture has been added to the service",
                          Dependencies = Array.Empty<DependencyBase>(),
                          ServiceImplementation = CreateServiceImplementation(
                              services => services
                                  .AddMockLogger(MockLoggerExtensions.CreateMockLogger())
                                  .AddKafkaBus(new KafkaSettings {Hosts = new[] {"localhost"}})
                          ),
                          ExpectedErrors
                              = new[]
                                {
                                    $"Message producer for topic {KafkaMessageProducerTypeExtensions.GetTopic(typeof(EntityCreationKafkaMessageProducer))} is not is not allowed by architecture",
                                    $"Message handler for topic {KafkaMessageHandlerTypeExtensions.GetTopic(typeof(EntityCreationKafkaMessageHandler))} is not is not allowed by architecture"
                                }
                      },
                      new DependencyValidatorTestCase
                      {
                          Description = "Kafka message producer and handler from the architecture were implemented in the service",
                          Dependencies
                              = new DependencyBase[]
                                {
                                    new KafkaDependency
                                    {
                                        Direction = MqDependencyDirection.Outgoing,
                                        Name = kafkaTopicName
                                    },
                                    new KafkaDependency
                                    {
                                        Direction = MqDependencyDirection.Incoming,
                                        Name = kafkaTopicName
                                    }
                                },
                          ServiceImplementation = CreateServiceImplementation(
                              services => services
                                  .AddMockLogger(MockLoggerExtensions.CreateMockLogger())
                                  .AddKafkaBus(new KafkaSettings {Hosts = new[] {"localhost"}})
                          ),
                          ExpectedErrors = Array.Empty<string>()
                      }
                  };
        }

        [Theory]
        [MemberData(nameof(KafkaDependenciesValidationTestCases))]
        public void ShouldValidateKafkaDependencies(DependencyValidatorTestCase testCase)
        {
            // Given
            var dependencyValidator = new KafkaDependencyValidator();

            // When
            var actualErrors = dependencyValidator.Validate(testCase.Dependencies, testCase.ServiceImplementation);

            // Then
            actualErrors.Should().BeEquivalentTo(testCase.ExpectedErrors);
        }
    }
}