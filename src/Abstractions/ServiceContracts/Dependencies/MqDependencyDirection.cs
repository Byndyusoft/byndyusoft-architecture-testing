namespace Byndyusoft.ArchitectureTesting.Abstractions.ServiceContracts.Dependencies
{
    using System.ComponentModel;

    /// <summary>
    /// Направление зависимости от очереди
    /// </summary>
    public enum MqDependencyDirection
    {
        Unknown = 0,

        /// <summary>
        /// Сервис получает данные из очереди
        /// </summary>
        [Description("Message handler")]
        Incoming = 1,

        /// <summary>
        /// Сервис отправляет данные в очередь
        /// </summary>
        [Description("Message producer")]
        Outgoing = 2
    }
}