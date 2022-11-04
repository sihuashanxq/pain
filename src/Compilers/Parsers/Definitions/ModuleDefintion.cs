using System;
namespace Pain.Compilers.Parsers.Definitions;

public class ModuleDefinition
{
    public string Path { get; }

    public Dictionary<string, ImportDefinition> Imports { get; }

    public Dictionary<string, ClassDefinition> Classes { get; }

    public ModuleDefinition(string path)
    {
        Path = path;
        Classes = new Dictionary<string, ClassDefinition>();
        Imports = new Dictionary<string, ImportDefinition>
        {
            ["Object"] = new ImportDefinition("Object", "Object", "Runtime"),
            ["Array"] = new ImportDefinition("Array", "Array", "Runtime"),
            ["String"] = new ImportDefinition("String", "String", "Runtime"),
            ["Number"] = new ImportDefinition("Number", "Number", "Runtime"),
            ["Boolean"] = new ImportDefinition("Boolean", "Boolean", "Runtime"),
            ["Console"] = new ImportDefinition("Console", "Console", "Runtime"),
        };
    }

    public override string ToString()
    {
        return string.Join("\n", Classes.Select(i => i.ToString()));
    }

    public void AddImported(params ImportDefinition[] imports)
    {
        foreach (var item in imports)
        {
            Imports[item.Alias] = item;
        }
    }

    public void AddClass(params ClassDefinition[] classes)
    {
        foreach (var item in classes)
        {
            Classes[item.Name] = item;
        }
    }

    public ModuleDefinition Initialize()
    {
        foreach (var item in Classes)
        {
            if (Classes.ContainsKey(item.Value.Super))
            {
                item.Value.Super = Path + "." + item.Value.Super;
                continue;
            }

            var super = Imports[item.Value.Super];
            item.Value.Super = super.Module + "." + super.Name;
        }

        return this;
    }
}

