namespace Byndyusoft.ArchitectureTesting.StructurizrParser.Tests
{
    using System;
    using System.IO;
    using Abstractions.ServiceContracts;
    using Abstractions.ServiceContracts.Dependencies;
    using FluentAssertions;
    using Xunit;

    public class StructurizrJsonParsingTestCase
    {
        public string Description { get; set; }

        public string JsonFileName { get; set; }

        public Func<string, bool> ServiceNameMatcher { get; set; }

        public ServiceContract[] ExpectedServiceContracts { get; set; }

        public override string ToString() => Description;
    }

    public class JsonParserTests
    {
        public static readonly TheoryData<StructurizrJsonParsingTestCase> StructurizrJsonParsingTestCases;

        static JsonParserTests()
        {
            StructurizrJsonParsingTestCases
                = new TheoryData<StructurizrJsonParsingTestCase>
                  {
                      new StructurizrJsonParsingTestCase
                      {
                          Description = "Should parse Musicality Labs architecture",
                          JsonFileName = "ArchitectureFiles/musicality-labs.json",
                          ServiceNameMatcher = x => x.StartsWith("musicality-labs", StringComparison.InvariantCultureIgnoreCase),
                          ExpectedServiceContracts
                              = new[]
                                {
                                    new ServiceContract
                                    {
                                        Name = "musicality-labs-storage-api",
                                        Dependencies
                                            = new DependencyBase[]
                                              {
                                                  new DbDependency {Name = "storageapi"}
                                              }
                                    },
                                    new ServiceContract
                                    {
                                        Name = "musicality-labs-storage-worker",
                                        Dependencies
                                            = new DependencyBase[]
                                              {
                                                  new RabbitDependency {Name = "musicality_labs.data.changes", Direction = MqDependencyDirection.Incoming},
                                                  new ApiDependency {Name = "musicality-labs-storage-api"}
                                              }
                                    }
                                }
                      }
                  };
        }

        [Theory]
        [MemberData(nameof(StructurizrJsonParsingTestCases))]
        public void ShouldParseStructurizrJson(StructurizrJsonParsingTestCase testCase)
        {
            // Given
            var jsonString = File.ReadAllText(testCase.JsonFileName);
            var parser = new JsonParser(testCase.ServiceNameMatcher);
           
            // When
            var actualServiceContracts = parser.Parse(jsonString);

            // Then
            actualServiceContracts.Should().BeEquivalentTo(testCase.ExpectedServiceContracts);
        }
    }
}