namespace Byndyusoft.ArchitectureTesting.StructurizrParser.Model
{
    internal static class RelationshipExtensions
    {
        public static bool IsSyncCall(this Relationship relationship) => relationship.HasTag("Sync");

        public static bool IsAsyncCall(this Relationship relationship) => relationship.HasTag("Async");
    }
}