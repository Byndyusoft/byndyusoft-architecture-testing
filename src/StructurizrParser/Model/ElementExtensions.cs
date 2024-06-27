namespace Byndyusoft.ArchitectureTesting.StructurizrParser.Model
{
    using System;

    internal static class ElementExtensions
    {
        public static bool IsWebApi(this Element element) => element.HasTag("WebApi");

        public static bool IsDatabase(this Element element) => element.HasTag("Database");

        public static bool IsMq(this Element element) => element.HasTag("MQ");

        private static string GetTechnology(this Element element) => (element.Technology ?? element.Metadata)!;

        public static bool IsMsSql(this Element element)
            => element.IsDatabase() && string.Equals(element.GetTechnology(), "MSSQL", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsPostgreSql(this Element element)
            => element.IsDatabase() && string.Equals(element.GetTechnology(), "PostgreSQL", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsS3(this Element element)
            => element.IsDatabase() && string.Equals(element.GetTechnology(), "MinIO", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsRabbit(this Element element)
            => element.IsMq() && string.Equals(element.GetTechnology(), "RabbitMQ", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsKafka(this Element element)
            => element.IsMq() && string.Equals(element.GetTechnology(), "Kafka", StringComparison.InvariantCultureIgnoreCase);
    }
}