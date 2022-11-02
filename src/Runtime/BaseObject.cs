namespace Pain.Runtime;

public class BaseObject
{
    private ClassObject _class { get; }

    private Dictionary<BaseObject, BaseObject> _fields { get; }

    public BaseObject(ClassObject @class)
    {
        _class = @class;
        _fields = new Dictionary<BaseObject, BaseObject>();
    }

    public virtual BaseObject GetField(BaseObject name)
    {
        if (_fields.TryGetValue(name, out var value))
        {
            return value;
        }

        if (_class.FunctionTable.TryGetFunction(name.ToString()!, out var function))
        {
            return new FunctionObject(this, function!);
        }

        var super = _class.Super;
        while (super != null)
        {
            if (super.FunctionTable.TryGetFunction(name.ToString()!, out function))
            {
                return new FunctionObject(this, function!);
            }

            super = super.Super;
        }

        return Builtin.Null;
    }

    public virtual void SetField(BaseObject name, BaseObject value)
    {
        _fields[name] = value;
    }

    public virtual bool IsTrue()
    {
        return true;
    }

    [Function("__equal__")]
    public virtual BaseObject __Euqal__(BaseObject obj)
    {
        return this == obj ? Builtin.True : Builtin.Flase;
    }

    [Function("__lessThan__")]
    public virtual BaseObject __LessThan__(BaseObject obj)
    {
        return Builtin.Flase;
    }

    public virtual BaseObject __GreaterThan__(BaseObject obj)
    {
        return Builtin.Flase;
    }

    public virtual BaseObject __LessThanOrEqual__(BaseObject obj)
    {
        return Builtin.Flase;
    }

    public virtual BaseObject __GtreaterThanOrEqual__(BaseObject obj)
    {
        return Builtin.Flase;
    }

    public virtual BaseObject __LeftShfit__(BaseObject obj)
    {
        return Builtin.Flase;
    }

    public virtual BaseObject __RightShfit__(BaseObject obj)
    {
        return Builtin.Flase;
    }

    public virtual BaseObject __Xor__(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject __Or__(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject __Not__()
    {
        throw new Exception();
    }

    public virtual BaseObject __And__(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject __Add__(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject __Sub__(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject __Mul__(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject __Mod__(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject __Div__(BaseObject obj)
    {
        throw new Exception();
    }

    public virtual BaseObject __Call__(BaseObject[] arguments)
    {
        throw new Exception();
    }
}

public class BooleanObject : BaseObject
{
    private bool _value;

    public BooleanObject(bool value) : base(Builtin.BooleanClass)
    {
        _value = value;
    }

    public override void SetField(BaseObject name, BaseObject value)
    {
        throw new Exception();
    }

    public override bool IsTrue()
    {
        return _value;
    }

    [Function("__equal__")]
    public override BaseObject __Euqal__(BaseObject obj)
    {
        return this.IsTrue() == obj.IsTrue() ? Builtin.True : Builtin.Flase;
    }
}

public class NumberObject : BaseObject
{
    private double _value;

    public NumberObject(double value) : base(Builtin.NumberClass)
    {
        _value = value;
    }

    public override void SetField(BaseObject name, BaseObject value)
    {
        throw new Exception();
    }
}

public class ArrayObject : BaseObject
{
    private List<BaseObject> _items;

    public ArrayObject() : base(Builtin.ArrayClass)
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
    public NullObject() : base(Builtin.NullClass)
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

    public FunctionObject(BaseObject target, Function function) : base(Builtin.FunctionClass)
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

public class Builtin
{
    public static ClassObject ObjectClass;

    public static ClassObject StringClass;

    public static ClassObject FunctionClass;

    public static ClassObject BooleanClass;

    public static ClassObject NumberClass;

    public static ClassObject NullClass;

    public static ClassObject ArrayClass;

    public static NullObject Null = new NullObject();

    public static BooleanObject True = new BooleanObject(true);

    public static BooleanObject Flase = new BooleanObject(false);
}

public class StringObject : BaseObject
{
    private string _value;

    public StringObject(string value, ClassObject @class) : base(@class)
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

    public override bool __LessThanOrEqual__(BaseObject obj)
    {
        return string.CompareOrdinal(_value, obj.ToString()) != 1;
    }

    public override bool __GtreaterThanOrEqual__(BaseObject obj)
    {
        return string.CompareOrdinal(_value, obj.ToString()) != -1;
    }

    public override BaseObject __And__(BaseObject obj)
    {
        return new RuntimeString(_value + obj.ToString(), _class);
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