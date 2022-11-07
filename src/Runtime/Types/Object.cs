namespace Pain.Runtime.Types;
using Pain.Runtime.VM;

public class Object : IObject
{
    public Type Type { get; }

    public Dictionary<IObject, IObject> Fields { get; }

    public Object(Type type)
    {
        Type = type;
        Fields = new Dictionary<IObject, IObject>();
    }

    public virtual Type GetType(VirtualMachine vm)
    {
        return Type;
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return true;
    }

    public IObject GetField(VirtualMachine vm, IObject key)
    {
        if (Fields.TryGetValue(key, out var value))
        {
            return value;
        }

        return GetType(vm).GetFunction(vm, this, key) as IObject ?? Null.Value;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value)
    {
        Fields[key] = value;
    }

    [Function("is")]
    public static IObject Is(IObject[] arguments)
    {
        if (arguments[0].GetType() == arguments[1].GetType())
        {
            return Boolean.True;
        }

        return Boolean.False;
    }

    [Function(Const.ConstructorFunc)]
    public static IObject Constructor(IObject[] arguments)
    {
        return Null.Value;
    }

    [Function(Const.ToStringFunc)]
    public static IObject ToString(IObject[] arguments)
    {
        return new String(string.Empty);
    }

    [Function(Const.EqualFunc)]
    public static IObject Euqal(IObject[] args)
    {
        if (args == null || args.Length != 2)
        {
            return Boolean.False;
        }

        if (args[0] == args[1])
        {
            return Boolean.True;
        }

        return Boolean.False;
    }

    [Function(Const.LessThanFunc)]
    public static IObject LessThan(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.GreaterThanFunc)]
    public static IObject GreaterThan(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.LessThanOrEqualFunc)]
    public static IObject LessThanOrEqual(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.GtreaterThanOrEqualFunc)]
    public static IObject GtreaterThanOrEqual(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.LeftShiftFunc)]
    public static IObject LeftShfit(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.RightShiftFunc)]
    public static IObject RightShift(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.XOrFunc)]
    public static IObject Xor(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.OrFunc)]
    public static IObject Or(IObject[] arguments)
    {

        throw new Exception();
    }

    [Function(Const.NotFunc)]
    public static IObject Not(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.AndFunc)]
    public static IObject And(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.AddFunc)]
    public static IObject Add(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.SubFunc)]
    public static IObject Sub(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.MulFunc)]
    public static IObject Mul(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.ModFunc)]
    public static IObject Mod(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.DivFunc)]
    public static IObject Div(IObject[] arguments)
    {
        throw new Exception();
    }

    [Function(Const.CallFunc)]
    public static IObject Call(IObject[] arguments)
    {
        throw new Exception();
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