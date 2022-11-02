namespace Pain.Runtime;

public class ClassLoader
{
    private readonly Dictionary<string, ClassObject> _items;

    public ClassLoader()
    {
        _items = new Dictionary<string, ClassObject>();
    }

    public ClassObject Load(string token)
    {
        if (_items.TryGetValue(token, out var metdata))
        {
            return metdata;
        }

        return null!;
    }
}