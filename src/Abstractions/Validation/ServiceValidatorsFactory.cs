namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation
{
    using System;

    /// <summary>
    /// Фабрика для создания валидаторов
    /// </summary>
    public static class ServiceValidatorsFactory
    {
        /// <summary>
        /// Создает валидатор реализации сервиса для переданной конфигурации <paramref name="config"/>
        /// </summary>
        /// <param name="config">Конфигурации валидатора</param>
        public static ServiceValidator Create(Action<IServiceValidatorBuilder> config)
        {
            var builder = new ServiceValidatorBuilder();
            config(builder);
            return new ServiceValidator(builder.Build());
        }
    }
}