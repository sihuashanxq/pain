namespace Pain.Runtime;

public class FunctionTable
{
    private Dictionary<string, Function> _functions { get; }

    public FunctionTable()
    {
        _functions = new Dictionary<string, Function>();
    }

    public bool TryGetFunction(string name, out Function? function)
    {
        if (_functions.TryGetValue(name, out function))
        {
            return true;
        }

        function = null;
        return false;
    }

    public void AddFunction(string name, Function function)
    {
        _functions[name] = function;
    }
}