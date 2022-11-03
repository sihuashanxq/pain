using System;
namespace Pain.Compilers.Parsers.Definitions;

public class ModuleDefinition
{
    public string Path { get; }
    
    public ImportDefinition[] Imports { get; }

    public List<ClassDefinition> Classes { get; }

    public ModuleDefinition(ClassDefinition[] classes, ImportDefinition[] imports)
    {
        Classes = new List<ClassDefinition>(classes);
        Imports = imports;
    }

    public override string ToString()
    {
        return string.Join("\n", Classes.Select(i => i.ToString()));
    }
}

