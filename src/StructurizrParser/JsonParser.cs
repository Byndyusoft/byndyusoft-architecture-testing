namespace Byndyusoft.ArchitectureTesting.StructurizrParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions.ServiceContracts;
    using Abstractions.ServiceContracts.Dependencies;
    using Extensions;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using StructurizrModel = Model.Model;

    /// <summary>
    /// Парсер JSON'а, генерируемого Structurizr
    /// </summary>
    public class JsonParser
    {
        private readonly Func<string, bool> _serviceNameMatcher;

        /// <summary>
        /// Инициализирует парсер
        /// </summary>
        /// <param name="serviceNameMatcher">Матчер, распознающий имя сервиса среди сегментов Url'а его репозитория</param>
        public JsonParser(Func<string, bool> serviceNameMatcher)
        {
            _serviceNameMatcher = serviceNameMatcher ?? throw new ArgumentNullException(nameof(serviceNameMatcher));
        }

        private static Workspace DeserializeJson(string jsonString)
            => JsonConvert.DeserializeObject<Workspace>(
                jsonString,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            )!;

        private static Relationship[] GetRelationships(IReadOnlyDictionary<int, Element> elementsByIds, StructurizrModel model)
            => model.CustomElements
                .SelectMany(x => x.Relationships)
                .Concat(
                    model.SoftwareSystems.SelectMany(
                        softwareSystem =>
                            softwareSystem.Containers.SelectMany(container => container.Relationships)
                    )
                )
                .Where(x => elementsByIds.ContainsKey(x.SourceId) && elementsByIds.ContainsKey(x.DestinationId))
                .ToArray();

        private static DependencyBase MapIncomingRelationshipToDependency(
            IReadOnlyDictionary<int, Element> elementsByIds,
            Relationship relationship
        )
        {
            var sourceElement = elementsByIds[relationship.SourceId];
            if (sourceElement.IsRabbit())
                return new RabbitDependency {Name = sourceElement.Name, Direction = MqDependencyDirection.Incoming};
            if (sourceElement.IsKafka())
                return new KafkaDependency {Name = sourceElement.Name, Direction = MqDependencyDirection.Incoming};

            throw new NotSupportedException(
                $"Can not parse relationship {relationship} from {sourceElement} to {elementsByIds[relationship.DestinationId]}"
            );
        }

        private string ExtractServiceNameFromUrl(string repositoryUrl)
            => repositoryUrl.Split(new[] {'\\', '/'}, StringSplitOptions.RemoveEmptyEntries).Single(_serviceNameMatcher);

        private DependencyBase MapOutgoingRelationshipToDependency(
            IReadOnlyDictionary<int, Element> elementsByIds,
            Relationship relationship
        )
        {
            var destinationElement = elementsByIds[relationship.DestinationId];
            if (relationship.IsSyncCall())
            {
                if (destinationElement.IsWebApi())
                    return new ApiDependency {Name = ExtractServiceNameFromUrl(destinationElement.Url!)};
                if (destinationElement.IsMsSql() || destinationElement.IsPostgreSql())
                    return new DbDependency {Name = destinationElement.Name};
                if (destinationElement.IsS3())
                    return new S3Dependency {Name = destinationElement.Name};
            }
            else
            {
                if (destinationElement.IsRabbit())
                    return new RabbitDependency {Name = destinationElement.Name, Direction = MqDependencyDirection.Outgoing};
                if (destinationElement.IsKafka())
                    return new KafkaDependency {Name = destinationElement.Name, Direction = MqDependencyDirection.Outgoing};
            }

            throw new NotSupportedException(
                $"Can not parse relationship {relationship} from {elementsByIds[relationship.SourceId]} to {destinationElement}"
            );
        }

        private ServiceContract MapToService(
            IReadOnlyDictionary<int, Element> elementsByIds,
            Element element,
            Relationship[]? incomingRelationships,
            Relationship[]? outgoingRelationships
        )
        {
            var service = new ServiceContract {Name = ExtractServiceNameFromUrl(element.Url!)};

            var dependencies = Enumerable.Empty<DependencyBase>();
            if (incomingRelationships != null)
                dependencies = dependencies.Concat(
                    incomingRelationships
                        .Where(x => x.IsAsyncCall())
                        .Select(x => MapIncomingRelationshipToDependency(elementsByIds, x))
                );
            if (outgoingRelationships != null)
                dependencies = dependencies.Concat(
                    outgoingRelationships.Select(x => MapOutgoingRelationshipToDependency(elementsByIds, x))
                );

            service.Dependencies = dependencies.ToArray();

            return service;
        }

        /// <summary>
        /// Парсит JSON-строку <paramref name="jsonString"/>, сгенерированную Structurizr, в объектную модель
        /// </summary>
        /// <param name="jsonString">JSON-строка, сгенерированная Structurizr</param>
        public ServiceContract[] Parse(string jsonString)
        {
            var model = DeserializeJson(jsonString).Model;
            var elementsByIds = model.CustomElements
                .Concat(model.SoftwareSystems.SelectMany(x => x.Containers))
                .ToDictionary(x => x.Id);
            var relationships = GetRelationships(elementsByIds, model);
            var relationsBySourceId = relationships
                .GroupBy(x => x.SourceId)
                .ToDictionary(x => x.Key, x => x.ToArray());
            var relationsByDestinationId = relationships
                .GroupBy(x => x.DestinationId)
                .ToDictionary(x => x.Key, x => x.ToArray());

            return model.SoftwareSystems
                .Where(x => x.IsExternalSystem() == false)
                .SelectMany(x => x.Containers)
                .Where(x => x.IsStorage() == false && x.IsMq() == false)
                .Select(
                    x => MapToService(
                        elementsByIds,
                        x,
                        relationsByDestinationId.GetValueOrDefault(x.Id),
                        relationsBySourceId.GetValueOrDefault(x.Id)
                    )
                )
                .ToArray();
        }
    }
}