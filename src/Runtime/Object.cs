namespace Pain.Runtime;
using System.Collections;
public class Object
{

    private Pain.Runtime.Metadata.Type _type;

    private Dictionary<Object, Object> _fields;

    public Object(Pain.Runtime.Metadata.Type type)
    {
        _type = type;
        _fields = new Dictionary<Object, Object>();
    }

    public virtual Object GetField(Object name)
    {
        return _fields[name];
    }

    public virtual void SetField(Object name, Object value)
    {
        _fields[name] = value;
    }
}

public class Array : Object
{
    private Object[] _items;

    public Array()
    {
        _items = new Object[0];
    }
}
