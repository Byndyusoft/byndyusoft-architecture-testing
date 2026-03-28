namespace Byndyusoft.ArchitectureTesting.RulesValidation.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    ///     Анализатор компонентов, из которых состоит приложение
    /// </summary>
    public interface IComponentsAnalyzer
    {
        /// <summary>
        ///     Анализирует переданные компоненты <paramref name="components" />
        /// </summary>
        /// <returns>Выявленные аномалии</returns>
        string[] Analyze(IReadOnlyList<Component> components);
    }
}