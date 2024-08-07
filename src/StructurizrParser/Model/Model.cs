namespace Byndyusoft.ArchitectureTesting.StructurizrParser.Model
{
    using System;

    internal class Model
    {
        public Element[] CustomElements { get; set; } = Array.Empty<Element>();

        public SoftwareSystem[] SoftwareSystems { get; set; } = Array.Empty<SoftwareSystem>();
    }
}