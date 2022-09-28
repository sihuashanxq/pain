using System.Collections.Generic;

namespace Pain.Compilers;

public class ConstantPool<T> where T : notnull
{
    private int _id;
    private readonly List<ConstantEntry<T>> _entries;

    private readonly Dictionary<T, ConstantEntry<T>> _items;

    public ConstantPool()
    {
        _items = new Dictionary<T, ConstantEntry<T>>();
        _entries = new List<ConstantEntry<T>>();
    }

    public int GetKey(T value)
    {
        if (!_items.TryGetValue(value, out var item))
        {
            item = new ConstantEntry<T>(Interlocked.Increment(ref _id), value);
            _items[value] = item;
        }

        return item.Key;
    }

    public T GetValue(int key)
    {
        return _entries[key].Value;
    }
}

public class ConstantEntry<T>
{
    public int Key { get; }

    public T Value { get; }

    public ConstantEntry(int key, T value)
    {
        Key = key;
        Value = value;
    }
}