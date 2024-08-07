namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ServiceContracts;

    public static class ServiceContractsCollectionExtensions
    {
        /// <summary>
        /// Находит в коллекции <paramref name="serviceContracts"/> описание сервиса по его корневой сборке <paramref name="rootAssembly"/>
        /// </summary>
        /// <param name="serviceContracts">Коллекция описаний сервисов</param>
        /// <param name="rootAssembly">Корневая сборка сервиса</param>
        public static ServiceContract FindServiceContract(this IEnumerable<ServiceContract> serviceContracts, Assembly rootAssembly)
            => serviceContracts.Single(x => rootAssembly.IsServiceRootAssembly(x.Name));
    }
}