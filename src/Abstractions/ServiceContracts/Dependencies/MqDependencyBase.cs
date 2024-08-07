namespace Byndyusoft.ArchitectureTesting.Abstractions.ServiceContracts.Dependencies
{
    /// <summary>
    /// Зависимость от очереди
    /// </summary>
    public abstract class MqDependencyBase : DependencyBase
    {
        /// <summary>
        /// Направление зависимости
        /// </summary>
        public MqDependencyDirection Direction { get; set; }
    }
}