namespace Byndyusoft.ArchitectureTesting.StructurizrParser.Model
{
    using System;

    internal class Element : ITagged
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tags { get; set; }

        public string? Metadata { get; set; }

        public string? Technology { get; set; }

        public string? Url { get; set; }

        public Relationship[] Relationships { get; set; } = Array.Empty<Relationship>();

        public override string ToString() => Name;
    }
}