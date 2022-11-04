namespace Pain.Runtime.Builtin;

[Class(Const.Runtime, Const.Object)]
public static class Object
{
    public static RuntimeClass Class { get; }

    public const string Token = $"{Const.Runtime}.{Const.Object}";

    static Object()
    {
        Class = new RuntimeClass(Const.Object,string.Empty, Const.Runtime,  Util.ScanFunctions(typeof(Object)));
    }


    [Function("is")]
    public static IObject Is(IObject[] arguments)
    {
        if (arguments[0].GetClass().Equals(arguments[1].GetClass()))
        {
            return new RuntimeBoolean(true);
        }

        return new RuntimeBoolean(false);
    }

    [Function(Const.ConstructorFunc)]
    public static IObject Constructor(IObject[] arguments)
    {
        return RuntimeNull.Null;
    }

    [Function(Const.ToStringFunc)]
    public static IObject ToString(IObject[] arguments)
    {
        return new RuntimeString(string.Empty);
    }

    [Function(Const.EqualFunc)]
    public static IObject Euqal(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeBoolean(false);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeBoolean(true);
        }

        return new RuntimeBoolean(false);
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