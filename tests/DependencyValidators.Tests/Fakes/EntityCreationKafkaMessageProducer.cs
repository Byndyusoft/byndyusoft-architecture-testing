namespace Byndyusoft.ArchitectureTesting.DependencyValidators.Tests.Fakes
{
    using Net.Kafka.Abstractions.Producing;
    using Net.Kafka.Producing;
    using System.Globalization;

    [KafkaMessageProducer(topic: "composer_assistant.entity.creation")]
    public class EntityCreationKafkaMessageProducer : KafkaMessageProducerBase<EntityCreationMessage>
    {
        public EntityCreationKafkaMessageProducer(IKafkaMessageSender messageSender) : base(messageSender)
        {
        }

        protected override string KeyGenerator(EntityCreationMessage message)
            => message.Id.ToString(CultureInfo.InvariantCulture);
    }
}