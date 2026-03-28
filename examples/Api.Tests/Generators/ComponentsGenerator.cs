namespace MusicalityLabs.Storage.Api.Tests.Generators;

using Byndyusoft.ArchitectureTesting.RulesValidation.Abstractions;

public static class ComponentsGenerator
{
    public static Component Create(params string[] componentPathSegments)
        => new()
           {
               Path = new ComponentsGraphPath(componentPathSegments),
               Children = [],
               Dependencies = []
           };

    public static Component SetChildren(this Component component, params Component[] children)
    {
        component.Children = children;
        return component;
    }
    
    public static Component SetDependencies(this Component component, params Component[] dependencies)
    {
        component.Dependencies = dependencies;
        return component;
    }
}