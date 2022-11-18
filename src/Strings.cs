namespace Pain;

public class Strings
{
    private int _id;

    private readonly List<StringEntry> _entries;

    private readonly Dictionary<string, StringEntry> _items;

    public Strings()
    {
        _items = new Dictionary<string, StringEntry>();
        _entries = new List<StringEntry>();
    }

    public int AddString(string value)
    {
        if (!_items.TryGetValue(value, out var item))
        {
            item = new StringEntry(_id, value);
            _items[value] = item;
            _entries.Add(item);
            _id++;
        }

        return item.Key;
    }

    public string GetString(int token)
    {
        return _entries[token].Value;
    }
}

public class StringEntry
{
    public int Key { get; }

    public string Value { get; }

    public StringEntry(int key, string value)
    {
        Key = key;
        Value = value;
    }
}
