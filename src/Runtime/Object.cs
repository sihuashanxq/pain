namespace Pain.Runtime;

public class BaseObject
{
    public static NullObject Null = null!;

    public ClassObject Class { get; }

    public Dictionary<BaseObject, BaseObject> Fields { get; }

    public BaseObject(ClassObject @class)
    {
        Class = @class;
        Fields = new Dictionary<BaseObject, BaseObject>();
    }

    public virtual BaseObject GetField(BaseObject name)
    {
        return Fields[name];
    }

    public virtual void SetField(BaseObject name, BaseObject value)
    {
        Fields[name] = value;
    }

    public virtual bool IsTrue()
    {
        return true;
    }

    [Function("__equal__")]
    public virtual BaseObject __Euqal__(BaseObject obj)
    {
        return this == obj;
    }

    [Function("__lessThan__")]
    public virtual BaseObject __LessThan__(BaseObject obj)
    {
        return false;
    }

    public virtual BaseObject __GreaterThan__(BaseObject obj)
    {
        return false;
    }

    public virtual BaseObject OperatorLessThanOrEqual(BaseObject obj)
    {
        return false;
    }

    public virtual BaseObject OperatorGreatherThanOrEqual(BaseObject obj)
    {
        return false;
    }

    public virtual BaseObject OperatorLeftShfit(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorRightShfit(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorXor(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorOr(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorNot()
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorAnd(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorAdd(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorSub(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorMul(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorMod(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorDiv(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject OperatorCall(BaseObject[] arguments)
    {
        throw new Exception();
    }
}

public class BooleanObject : BaseObject
{
    private bool _value;

    public BooleanObject(bool value, ClassObject.Metadata type) : base(type)
    {
        _value = value;
    }

    public override BaseObject GetField(BaseObject name)
    {
        throw new Exception();
    }

    public override void SetField(BaseObject name, BaseObject value)
    {
        throw new Exception();
    }
}

public class NumberObject : BaseObject
{
    private double _value;

    public NumberObject(double value, ClassObject.Metadata type) : base(type)
    {
        _value = value;
    }

    public override BaseObject GetField(BaseObject name)
    {
        throw new Exception();
    }

    public override void SetField(BaseObject name, BaseObject value)
    {
        throw new Exception();
    }
}

public class ArrayObject : BaseObject
{
    private List<BaseObject> _items;

    public ArrayObject(ClassObject.Metadata type) : base(type)
    {
        _items = new List<BaseObject>();
    }

    public override BaseObject GetField(BaseObject name)
    {
        throw new Exception();
    }

    public override void SetField(BaseObject name, BaseObject value)
    {
        throw new Exception();
    }
}

public class NullObject : BaseObject
{
    public NullObject(double value, ClassObject.Metadata type) : base(type)
    {
    }

    public override BaseObject GetField(BaseObject name)
    {
        throw new Exception();
    }

    public override void SetField(BaseObject name, BaseObject value)
    {
        throw new Exception();
    }
}

public class FunctionObject : BaseObject
{
    public Function Function { get; }

    public BaseObject Target { get; }

    public FunctionObject(BaseObject target, Function function) : base(null!)
    {
        Target = target;
        Function = function;
    }

    public override BaseObject GetField(BaseObject name)
    {
        throw new Exception();
    }

    public override void SetField(BaseObject name, BaseObject value)
    {
        throw new Exception();
    }
}

public class ClassObject : BaseObject
{
    public string Name { get; }

    public string Module { get; }

    public string Token { get; }

    public ClassObject Super { get; }

    public FunctionTable FunctionTable { get; }

    public ClassObject(ClassObject super, string name, string module, FunctionTable functionTable) : base(null)
    {
        Name = name;
        Super = super;
        Token = $"{module}.{name}";
        Module = module;
        FunctionTable = functionTable;
    }

    public BaseObject CreateInstance()
    {
        return new BaseObject(this);
    }
}

public class StringObject : BaseObject
{
    private string _value;

    public StringObject(string value, ClassObject.Metadata type) : base(type)
    {
        _value = value;
    }

    public override BaseObject GetField(BaseObject name)
    {
        throw new Exception();
    }

    public override void SetField(BaseObject name, BaseObject value)
    {
        throw new Exception();
    }

    public override bool IsTrue()
    {
        return !string.IsNullOrEmpty(_value);
    }

    public override bool __Euqal__(BaseObject obj)
    {
        return _value == obj.ToString();
    }

    public override bool __LessThan__(BaseObject obj)
    {
        return string.CompareOrdinal(_value, obj.ToString()) == -1;
    }

    public override bool __GreaterThan__(BaseObject obj)
    {
        return string.CompareOrdinal(_value, obj.ToString()) == 1;
    }

    public override bool OperatorLessThanOrEqual(BaseObject obj)
    {
        return string.CompareOrdinal(_value, obj.ToString()) != 1;
    }

    public override bool OperatorGreatherThanOrEqual(BaseObject obj)
    {
        return string.CompareOrdinal(_value, obj.ToString()) != -1;
    }

    public override BaseObject OperatorAdd(BaseObject obj)
    {
        return new RuntimeString(_value + obj.ToString(), Class);
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