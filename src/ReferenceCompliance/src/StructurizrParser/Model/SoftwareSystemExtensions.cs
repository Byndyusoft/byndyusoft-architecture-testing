namespace Byndyusoft.ArchitectureTesting.ReferenceCompliance.StructurizrParser.Model
{
    internal static class SoftwareSystemExtensions
    {
        public static bool IsExternalSystem(this SoftwareSystem softwareSystem) => softwareSystem.HasTag("ExternalSystem");
    }
}