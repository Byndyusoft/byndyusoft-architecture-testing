namespace Byndyusoft.ArchitectureTesting.ReferenceCompliance.DependencyValidators.Tests.Fakes;

using System.Globalization;
using Byndyusoft.Net.Kafka.Abstractions.Producing;
using Byndyusoft.Net.Kafka.Producing;

[KafkaMessageProducer("composer_assistant.entity.creation")]
public class EntityCreationKafkaMessageProducer : KafkaMessageProducerBase<EntityCreationMessage>
{
    public EntityCreationKafkaMessageProducer(IKafkaMessageSender messageSender) : base(messageSender)
    {
    }

    protected override string KeyGenerator(EntityCreationMessage message)
        => message.Id.ToString(CultureInfo.InvariantCulture);
}