namespace Pain.Runtime;
using Pain.Runtime.Metadata;

public class RuntimeObject
{
    public static RuntimeNull Null;

    public static RuntimeBoolean True;

    public static RuntimeBoolean False;

    public MetadataType Type { get; }

    public Dictionary<RuntimeObject, RuntimeObject> Fields { get; }

    public RuntimeObject(MetadataType type)
    {
        Type = type;
        Fields = new Dictionary<RuntimeObject, RuntimeObject>();
    }

    public virtual RuntimeObject GetField(RuntimeObject name)
    {
        return Fields[name];
    }

    public virtual void SetField(RuntimeObject name, RuntimeObject value)
    {
        Fields[name] = value;
    }

    public virtual bool IsTrue()
    {
        return true;
    }

    [Function("__equal__")]
    public virtual RuntimeObject OperatorEqual(RuntimeObject obj)
    {
        return this == obj;
    }

    [Function("__lessThan__")]
    public virtual RuntimeObject OperatorLessThan(RuntimeObject obj)
    {
        return false;
    }

    public virtual RuntimeObject OperatorGreatherThan(RuntimeObject obj)
    {
        return false;
    }

    public virtual RuntimeObject OperatorLessThanOrEqual(RuntimeObject obj)
    {
        return false;
    }

    public virtual RuntimeObject OperatorGreatherThanOrEqual(RuntimeObject obj)
    {
        return false;
    }

    public virtual RuntimeObject OperatorLeftShfit(RuntimeObject obj)
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorRightShfit(RuntimeObject obj)
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorXor(RuntimeObject obj)
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorOr(RuntimeObject obj)
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorNot()
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorAnd(RuntimeObject obj)
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorAdd(RuntimeObject obj)
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorSub(RuntimeObject obj)
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorMul(RuntimeObject obj)
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorMod(RuntimeObject obj)
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorDiv(RuntimeObject obj)
    {
        throw new Exception();
    }

    public virtual RuntimeObject OperatorCall(RuntimeObject[] arguments)
    {
        throw new Exception();
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
    private List<RuntimeObject> _items;

    public RuntimeArray(MetadataType type) : base(type)
    {
        _items = new List<RuntimeObject>();
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
    public Function Metadata { get; }

    public RuntimeObject Target { get; }

    public MemoryStream OpCodes { get; }

    public RuntimeFunction(RuntimeObject target, Function function) : base(null!)
    {
        Target = target;
        OpCodes = new MemoryStream(function.OpCodes);
        Metadata = function;
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

    public override bool IsTrue()
    {
        return !string.IsNullOrEmpty(_value);
    }

    public override bool OperatorEqual(RuntimeObject obj)
    {
        return _value == obj.ToString();
    }

    public override bool OperatorLessThan(RuntimeObject obj)
    {
        return string.CompareOrdinal(_value, obj.ToString()) == -1;
    }

    public override bool OperatorGreatherThan(RuntimeObject obj)
    {
        return string.CompareOrdinal(_value, obj.ToString()) == 1;
    }

    public override bool OperatorLessThanOrEqual(RuntimeObject obj)
    {
        return string.CompareOrdinal(_value, obj.ToString()) != 1;
    }

    public override bool OperatorGreatherThanOrEqual(RuntimeObject obj)
    {
        return string.CompareOrdinal(_value, obj.ToString()) != -1;
    }

    public override RuntimeObject OperatorAdd(RuntimeObject obj)
    {
        return new RuntimeString(_value + obj.ToString(), Type);
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class FunctionAttribute : Attribute
{
    public string Name { get; }

    public FunctionAttribute(string name)
    {
        Name = name;
    }
}