namespace Byndyusoft.ArchitectureTesting.StructurizrParser.Model
{
    internal class Relationship : ITagged
    {
        public int SourceId { get; set; }

        public int DestinationId { get; set; }

        public string Description { get; set; }

        public string Tags { get; set; }

        public string Technology { get; set; }

        public override string ToString() => Description;
    }
}