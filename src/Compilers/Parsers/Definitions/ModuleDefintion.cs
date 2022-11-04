using System;
namespace Pain.Compilers.Parsers.Definitions;

public class ModuleDefinition
{
    public string Path { get; }

    public List<ImportDefinition> Imports { get; }

    public List<ClassDefinition> Classes { get; }

    public ModuleDefinition(string path)
    {
        Path = path;
        Classes = new List<ClassDefinition>();
        Imports = new List<ImportDefinition>(){
            new ImportDefinition("Runtime",new[]{
                new ImportClass("Object","Object"),
                new ImportClass("String","String"),
                new ImportClass("Number","Number"),
                new ImportClass("Boolean","Boolean"),
            })
        };
    }

    public override string ToString()
    {
        return string.Join("\n", Classes.Select(i => i.ToString()));
    }

    public void AddImportedClass(ImportDefinition importDefinition)
    {
        Imports.Add(importDefinition);
    }

    public void AddClass(ClassDefinition classDefinition)
    {
        Classes.Add(classDefinition);
    }
}

