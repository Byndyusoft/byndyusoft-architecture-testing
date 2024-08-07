
namespace Byndyusoft.ArchitectureTesting.Abstractions.ServiceContracts
{
    using Dependencies;

    /// <summary>
    /// Описание сервиса
    /// </summary>
    public class ServiceContract
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Зависимости
        /// </summary>
        public DependencyBase[] Dependencies { get; set; }

        public override string ToString() => Name;
    }
}