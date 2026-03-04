namespace Byndyusoft.ArchitectureTesting.RulesValidation.Abstractions
{
    using System;

    /// <summary>
    ///     Путь в графе компонентов
    /// </summary>
    public class ComponentsGraphPath
    {
        /// <summary>
        ///     Инициализация зависимостей объекта
        /// </summary>
        /// <param name="segments">Сегменты пути в графе компонентов</param>
        public ComponentsGraphPath(string[] segments)
        {
            Segments = segments;
            Text = string.Join(".", segments);
        }

        /// <summary>
        ///     Сегменты, из которых состоит путь в графе
        /// </summary>
        public string[] Segments { get; }

        /// <summary>
        ///     Путь в графе в виде одной строки
        /// </summary>
        public string Text { get; }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;

            return Equals((ComponentsGraphPath)obj);
        }

        public override int GetHashCode()
            => StringComparer.InvariantCultureIgnoreCase.GetHashCode(Text);

        public override string ToString() => Text;

        private bool Equals(ComponentsGraphPath other)
            => string.Equals(Text, other.Text, StringComparison.InvariantCultureIgnoreCase);
    }
}