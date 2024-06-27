namespace Byndyusoft.ArchitectureTesting.Abstractions.ServiceContracts.Dependencies
{
    /// <summary>
    /// Описание зависимости сервиса
    /// </summary>
    public abstract class DependencyBase
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}