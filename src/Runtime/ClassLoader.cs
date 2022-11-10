namespace Pain.Runtime;
using RuntimeType = Pain.Runtime.Types.Type;
public class ClassLoader
{
    private readonly Dictionary<ModuleToken, RuntimeType> _items;

    private readonly Func<ModuleToken, IEnumerable<RuntimeType>> _compile;

    public ClassLoader(Func<ModuleToken, IEnumerable<RuntimeType>> compile)
    {
        _items = new Dictionary<ModuleToken, RuntimeType>();
        _compile = compile;
    }

    public RuntimeType Load(ModuleToken token)
    {
        if (_items.TryGetValue(token, out var rClass))
        {
            return rClass;
        }

        foreach (var item in _compile(token))
        {
            _items[item.Token] = item;
        }

        return Load(token);
    }
}