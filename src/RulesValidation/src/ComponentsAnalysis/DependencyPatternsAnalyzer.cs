namespace Byndyusoft.ArchitectureTesting.RulesValidation.ComponentsAnalysis
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;

    /// <summary>
    ///     Анализатор паттернов зависимостей между компонентами
    /// </summary>
    public class DependencyPatternsAnalyzer : IComponentsAnalyzer
    {
        /// <inheritdo />
        public string[] Analyze(IReadOnlyList<Component> components)
            => components
                .SelectMany(
                    dependencyOrigin => dependencyOrigin.Dependencies
                        .Where(dependencyTarget => DoesMatchKnownDependencyPatterns(dependencyOrigin, dependencyTarget) == false)
                        .Select(dependencyTarget => $"Component {dependencyOrigin.Path} depends on {dependencyTarget.Path}")
                )
                .ToArray();

        private static bool DoesMatchKnownDependencyPatterns(Component dependencyOrigin, Component dependencyTarget)
            => DoesDependOnChildComponent(dependencyOrigin, dependencyTarget)
               || DoesDependOnSharedComponentOrItsChild(dependencyOrigin, dependencyTarget);

        private static bool DoesDependOnChildComponent(Component dependencyOrigin, Component dependencyTarget)
            => dependencyOrigin.Path.Contains(dependencyTarget.Path)
               && dependencyTarget.Path.CalculateDistanceFromCommonAncestor(dependencyOrigin.Path) == 1;

        private static bool DoesDependOnSharedComponentOrItsChild(Component dependencyOrigin, Component dependencyTarget)
            => dependencyOrigin.Path.Contains(dependencyTarget.Path) == false
               && dependencyTarget.Path.CalculateDistanceFromCommonAncestor(dependencyOrigin.Path) <= 2;
    }
}