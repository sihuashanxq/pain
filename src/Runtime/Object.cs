namespace Pain.Runtime;
using Pain.Runtime.Metadata;

public class RuntimeObject
{
    private MetadataType _type;

    private Dictionary<RuntimeObject, RuntimeObject> _fields;

    public RuntimeObject(MetadataType type)
    {
        _type = type;
        _fields = new Dictionary<RuntimeObject, RuntimeObject>();
    }

    public virtual RuntimeObject GetField(RuntimeObject name)
    {
        return _fields[name];
    }

    public virtual void SetField(RuntimeObject name, RuntimeObject value)
    {
        _fields[name] = value;
    }

    public virtual bool __Truth__()
    {
        return true;
    }
}

public class RuntimeBoolean : RuntimeObject
{
    private bool _value;

    public RuntimeBoolean(bool value, MetadataType type) : base(type)
    {
        _value = value;
    }

    public override RuntimeObject GetField(RuntimeObject name)
    {
        throw new Exception();
    }

    public override void SetField(RuntimeObject name, RuntimeObject value)
    {
        throw new Exception();
    }
}

public class RuntimeNumber : RuntimeObject
{
    private double _value;

    public RuntimeNumber(double value, MetadataType type) : base(type)
    {
        _value = value;
    }

    public override RuntimeObject GetField(RuntimeObject name)
    {
        throw new Exception();
    }

    public override void SetField(RuntimeObject name, RuntimeObject value)
    {
        throw new Exception();
    }
}

public class RuntimeArray : RuntimeObject
{
    private List<Object> _items;

    public RuntimeArray(MetadataType type) : base(type)
    {
        _items = new List<object>();
    }

    public override RuntimeObject GetField(RuntimeObject name)
    {
        throw new Exception();
    }

    public override void SetField(RuntimeObject name, RuntimeObject value)
    {
        throw new Exception();
    }
}

public class RuntimeNull : RuntimeObject
{
    public RuntimeNull(double value, MetadataType type) : base(type)
    {
    }

    public override RuntimeObject GetField(RuntimeObject name)
    {
        throw new Exception();
    }

    public override void SetField(RuntimeObject name, RuntimeObject value)
    {
        throw new Exception();
    }
}

public class RuntimeFunction : RuntimeObject
{
    public Function Function { get; }

    public RuntimeObject Target { get; }

    public RuntimeFunction(RuntimeObject target, Function function) : base(null!)
    {
        Target = target;
        Function = function;
    }

    public override RuntimeObject GetField(RuntimeObject name)
    {
        throw new Exception();
    }

    public override void SetField(RuntimeObject name, RuntimeObject value)
    {
        throw new Exception();
    }
}

public class RuntimeString : RuntimeObject
{
    private string _value;

    public RuntimeString(string value, MetadataType type) : base(type)
    {
        _value = value;
    }

    public override RuntimeObject GetField(RuntimeObject name)
    {
        throw new Exception();
    }

    public override void SetField(RuntimeObject name, RuntimeObject value)
    {
        throw new Exception();
    }
}