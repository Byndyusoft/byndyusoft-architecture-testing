namespace MusicalityLabs.Storage.Api.Tests;

using Byndyusoft.ArchitectureTesting.RulesValidation.ComponentsAnalysis;
using FluentAssertions;
using Generators;
using Validation.ComponentsExtraction;
using Xunit;

public class ComponentsStructureTest
{
    [Fact]
    public void ShouldExtractComponentsCorrectly()
    {
        // Given
        var expectedApiComponent = ComponentsGenerator.Create("Api");
        var expectedApiContractsComponent = ComponentsGenerator.Create("Api", "Contracts.SoundSignatures");
        var expectedDataAccessComponent = ComponentsGenerator.Create("DataAccess.SoundSignatures");

        var expectedComponents
            = new[]
              {
                  expectedApiContractsComponent,
                  expectedApiComponent
                      .SetChildren(expectedApiContractsComponent)
                      .SetDependencies(expectedDataAccessComponent),
                  expectedDataAccessComponent
                      .SetDependencies(expectedApiContractsComponent)
              };

        // When
        var actualComponents = new ComponentsExtractor().Extract();

        // Then
        actualComponents.Should().BeEquivalentTo(expectedComponents);
    }

    [Fact]
    public void ShouldDetectOnlyKnownDependencyPatterns()
    {
        // Given
        var components = new ComponentsExtractor().Extract();
        
        // When
        var anomalies = new DependencyPatternsAnalyzer().Analyze(components);

        // Then
        anomalies.Should().BeEmpty();
    }
}