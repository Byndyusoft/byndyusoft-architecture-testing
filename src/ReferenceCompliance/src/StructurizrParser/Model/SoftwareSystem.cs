namespace Byndyusoft.ArchitectureTesting.ReferenceCompliance.StructurizrParser.Model
{
    internal class SoftwareSystem : ITagged
    {
        public string Name { get; set; }

        public Element[] Containers { get; set; }

        public string Tags { get; set; }

        public override string ToString() => Name;
    }
}