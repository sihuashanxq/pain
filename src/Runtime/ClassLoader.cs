namespace Pain.Runtime;

public class ClassLoader
{
    private readonly Func<string, IEnumerable<RuntimeClass>> _compile;

    private readonly Dictionary<string, RuntimeClass> _items;

    public ClassLoader(Func<string, IEnumerable<RuntimeClass>> compile)
    {
        _items = new Dictionary<string, RuntimeClass>()
        {
            [$"{Builtin.Const.Runtime}.{Builtin.Const.Null}"] = Builtin.Null.Class,
            [$"{Builtin.Const.Runtime}.{Builtin.Const.Array}"] = Builtin.Array.Class,
            [$"{Builtin.Const.Runtime}.{Builtin.Const.Object}"] = Builtin.Object.Class,
            [$"{Builtin.Const.Runtime}.{Builtin.Const.String}"] = Builtin.String.Class,
            [$"{Builtin.Const.Runtime}.{Builtin.Const.Number}"] = Builtin.Number.Class,
            [$"{Builtin.Const.Runtime}.{Builtin.Const.Boolean}"] = Builtin.Boolean.Class,
        };
        _compile = compile;
    }

    public RuntimeClass Load(string token)
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