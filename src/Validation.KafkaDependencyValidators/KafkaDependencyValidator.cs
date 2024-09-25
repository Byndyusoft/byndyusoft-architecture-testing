
namespace Byndyusoft.ArchitectureTesting.Validation.KafkaDependencyValidators
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions.ServiceContracts.Dependencies;
    using Abstractions.ServiceImplementations;
    using Abstractions.Validation.DependencyValidators;
    using Byndyusoft.ArchitectureTesting.Abstractions.Validation.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Net.Kafka.Abstractions.Consuming;
    using Net.Kafka.Abstractions.Producing;

    public class KafkaDependencyValidator : DependencyValidatorBase<KafkaDependency>
    {
        private static IEnumerable<string> Validate(
            MqDependencyDirection mqDependencyDirection,
            string[] expectedTopics,
            string[] actualTopics
        )
        {
            var mqDirectionDescription = mqDependencyDirection.GetDescription();
            return Enumerable.Empty<string>()
                .Concat(
                    expectedTopics.Except(actualTopics)
                        .Select(missedTopic => $"{mqDirectionDescription} for topic {missedTopic} is missed")
                )
                .Concat(
                    actualTopics.Except(expectedTopics)
                        .Select(excessTopic => $"{mqDirectionDescription} for topic {excessTopic} is not is not allowed by architecture")
                );
        }

        protected override IEnumerable<string> Validate(KafkaDependency[] dependencies, ServiceImplementation serviceImplementation)
        {
            var expectedIncomingTopics = dependencies
                .Where(x => x.Direction == MqDependencyDirection.Incoming)
                .Select(x => x.Name)
                .ToArray();
            var expectedOutgoingTopics = dependencies
                .Where(x => x.Direction == MqDependencyDirection.Outgoing)
                .Select(x => x.Name)
                .ToArray();

            var actualIncomingTopics = serviceImplementation.ServiceProvider.GetServices<IKafkaMessageHandler>()
                .Select(x => KafkaMessageHandlerTypeExtensions.GetTopic(x.GetType()))
                .ToArray();
            var actualOutgoingTopics = serviceImplementation.ServiceProvider.GetServices<IKafkaMessageProducer>()
                .Select(x => KafkaMessageProducerTypeExtensions.GetTopic(x.GetType()))
                .ToArray();

            return Enumerable.Empty<string>()
                .Concat(Validate(MqDependencyDirection.Incoming, expectedIncomingTopics, actualIncomingTopics))
                .Concat(Validate(MqDependencyDirection.Outgoing, expectedOutgoingTopics, actualOutgoingTopics));
        }
    }
}