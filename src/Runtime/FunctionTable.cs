namespace Pain.Runtime;

public class FunctionTable
{
    private Dictionary<string, CompiledFunction> _functions { get; }

    public FunctionTable()
    {
        _functions = new Dictionary<string, CompiledFunction>();
    }

    public void Add(string name, CompiledFunction function)
    {
        _functions[name] = function;
    }

    public void Add(FunctionTable table)
    {
        foreach (var item in table)
        {
            if (item.Native)
            {
                if (!TryGet(item.Name, out var exists) || !exists!.Native)
                {
                    throw new Exception();
                }
            }
            else
            {
                if (TryGet(item.Name, out var _))
                {
                    continue;
                }

                Add(item.Name, item);
            }
        }
    }

    public bool TryGet(string name, out CompiledFunction? function)
    {
        if (_functions.TryGetValue(name, out function))
        {
            return true;
        }

        function = null;
        return false;
    }

    public IEnumerator<CompiledFunction> GetEnumerator()
    {
        return _functions.Values.GetEnumerator();
    }
}