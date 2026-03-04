namespace Byndyusoft.ArchitectureTesting.RulesValidation.Abstractions
{
    /// <summary>
    ///     Компонент, из которых состоит приложение
    /// </summary>
    public class Component
    {
        /// <summary>
        ///     Путь до компонента в графе
        /// </summary>
        public ComponentsGraphPath Path { get; set; }

        /// <summary>
        ///     Компоненты, являющиеся непосредственным частями данного
        /// </summary>
        public Component[] Children { get; set; }

        /// <summary>
        ///     Компоненты, от которых зависит данный
        /// </summary>
        public Component[] Dependencies { get; set; }

        public override string ToString() => Path.Text;
    }
}