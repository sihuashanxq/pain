namespace Pain.Runtime;

public class FunctionTable
{
    private Dictionary<string, CompiledFunction> _functions { get; }

    public FunctionTable()
    {
        _functions = new Dictionary<string, CompiledFunction>();
    }

    public bool TryGetFunction(string name, out CompiledFunction? function)
    {
        if (_functions.TryGetValue(name, out function))
        {
            return true;
        }

        function = null;
        return false;
    }

    public void AddFunction(string name, CompiledFunction function)
    {
        _functions[name] = function;
    }
}