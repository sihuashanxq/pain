namespace Pain.Runtime;

public class FunctionTable
{
    public Dictionary<string, Function> Functions { get; }

    public FunctionTable()
    {
        Functions = new Dictionary<string, Function>();
    }

    public Function? GetFunction(string name)
    {
        if (Functions.TryGetValue(name, out var function))
        {
            return function;
        }

        return null;
    }

    public void AddFunction(string name, Function function)
    {
        Functions[name] = function;
    }
}