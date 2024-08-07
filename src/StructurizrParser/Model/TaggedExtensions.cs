namespace Byndyusoft.ArchitectureTesting.StructurizrParser.Model
{
    using System;
    using System.Linq;

    internal static class TaggedExtensions
    {
        public static bool HasTag(this ITagged tagged, string tag)
            => tagged.Tags.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                     .Select(x => x.Trim())
                     .Any(x => x.Equals(tag, StringComparison.InvariantCultureIgnoreCase));
    }
}