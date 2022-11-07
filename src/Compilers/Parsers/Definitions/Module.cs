namespace Pain.Compilers.Parsers.Definitions;

public class Module
{
    public string Path { get; }

    public Dictionary<string, Class> Classes { get; }

    public Dictionary<string, Import> Imports { get; }

    public Module(string path)
    {
        Path = path;
        Classes = new Dictionary<string, Class>();
        Imports = new Dictionary<string, Import>()
        {
            ["Object"] = new Import("Object", "Object", "@Runtime"),
            ["Array"] = new Import("Object", "Array", "@Runtime"),
        };
    }

    public override string ToString()
    {
        return string.Join("\n", Classes.Select(i => i.ToString()));
    }

    public void AddImported(params Import[] imports)
    {
        foreach (var item in imports)
        {
            Imports[item.Alias] = item;
        }
    }

    public void AddClass(params Class[] classes)
    {
        foreach (var item in classes)
        {
            Classes[item.Name] = item;
        }
    }
}
