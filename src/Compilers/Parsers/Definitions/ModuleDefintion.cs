using System;
namespace Pain.Compilers.Parsers.Definitions;

public class ModuleDefinition
{
    public ClassDefinition[] Classes { get; }

    public ImportDefinition[] Imports { get; }

    public ModuleDefinition(ClassDefinition[] classes, ImportDefinition[] imports)
    {
        Classes = classes;
        Imports = imports;
    }
}

