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

    public virtual Type GetType(VirtualMachine vm, out bool @throw)
    {
        @throw = false;
        return Type;
    }

    public bool ToBoolean(VirtualMachine vm)
    {
        return true;
    }

    public IObject GetField(VirtualMachine vm, IObject key, out bool @throw)
    {
        @throw = false;
        if (Fields.TryGetValue(key, out var value))
        {
            return value;
        }

        return GetType(vm, out @throw).GetFunction(vm, this, key) as IObject ?? Null.Value;
    }

    public void SetField(VirtualMachine vm, IObject key, IObject value, out bool @throw)
    {
        @throw = false;
        Fields[key] = value;
    }

    [Function("is")]
    public static IObject Is(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        if (arguments[0].GetType(null, out @throw) == arguments[1].GetType(null, out @throw))
        {
            return Boolean.True;
        }

        return Boolean.False;
    }

    [Function(Const.ConstructorFunc)]
    public static IObject Constructor(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        return Null.Value;
    }

    [Function(Const.ToStringFunc)]
    public static IObject ToString(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        return new String(string.Empty);
    }

    [Function(Const.EqualFunc)]
    public static IObject Euqal(IObject[] args, out bool @throw)
    {
        @throw = false;
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
    public static IObject LessThan(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.GreaterThanFunc)]
    public static IObject GreaterThan(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.LessThanOrEqualFunc)]
    public static IObject LessThanOrEqual(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.GtreaterThanOrEqualFunc)]
    public static IObject GtreaterThanOrEqual(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.LeftShiftFunc)]
    public static IObject LeftShfit(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.RightShiftFunc)]
    public static IObject RightShift(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.XOrFunc)]
    public static IObject Xor(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.OrFunc)]
    public static IObject Or(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.NotFunc)]
    public static IObject Not(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.AndFunc)]
    public static IObject And(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.AddFunc)]
    public static IObject Add(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.SubFunc)]
    public static IObject Sub(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.MulFunc)]
    public static IObject Mul(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.ModFunc)]
    public static IObject Mod(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.DivFunc)]
    public static IObject Div(IObject[] arguments, out bool @throw)
    {
        @throw = false;
        throw new Exception();
    }

    [Function(Const.CallFunc)]
    public static IObject Call(IObject[] arguments, out bool @throw)
    {
        @throw = false;
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