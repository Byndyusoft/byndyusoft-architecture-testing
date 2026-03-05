namespace MusicalityLabs.Storage.Api.Tests.Validation.ServiceImplementations;

using System.Reflection;
using Byndyusoft.ArchitectureTesting.Common.Abstractions.Extensions;
using Byndyusoft.ArchitectureTesting.ReferenceCompliance.ServiceImplementations;

public class ServiceImplementationsFactory : ServiceImplementationFactoryBase<Program>
{
    protected override bool IsImplementationAssembly(AssemblyName assemblyName)
        => assemblyName.FullNameStartsWith("Byndyusoft.", "MusicalityLabs.");
}