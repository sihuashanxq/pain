namespace Pain.Runtime.Builtin;

[Class(Const.Runtime, Const.Number)]
public static class Number
{
    public static NumberClass Class { get; }

    static Number()
    {
        Class = new NumberClass(Const.Runtime, Const.Object, Util.ScanFunctions(typeof(Number)));
    }

    public static bool IsNumber(IObject v2)
    {
        return v2 is Runtime.Number;
    }

    [Function(Const.ToStringFunc)]
    public static IObject ToString(IObject[] arguments)
    {
        if (arguments == null || arguments.Length == 0)
        {
            throw new ArgumentException();
        }

        return new Runtime.String(((Runtime.Number)arguments[0]).Value.ToString());
    }

    [Function(Const.EqualFunc)]
    public static IObject Euqal(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Boolean(false);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Boolean(true);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Boolean(((Runtime.Number)arguments[0]).Value == ((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Boolean(false);
    }

    [Function(Const.LessThanFunc)]
    public static IObject LessThan(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Boolean(false);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Boolean(true);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Boolean(((Runtime.Number)arguments[0]).Value < ((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Boolean(false);
    }

    [Function(Const.GreaterThanFunc)]
    public static IObject GreaterThan(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Boolean(false);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Boolean(true);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Boolean(((Runtime.Number)arguments[0]).Value > ((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Boolean(false);
    }

    [Function(Const.LessThanOrEqualFunc)]
    public static IObject LessThanOrEqual(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Boolean(false);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Boolean(true);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Boolean(((Runtime.Number)arguments[0]).Value <= ((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Boolean(false);
    }

    [Function(Const.GtreaterThanOrEqualFunc)]
    public static IObject GtreaterThanOrEqual(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Boolean(false);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Boolean(true);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Boolean(((Runtime.Number)arguments[0]).Value >= ((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Boolean(false);
    }

    [Function(Const.LeftShiftFunc)]
    public static IObject LeftShfit(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Number(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Number(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Number((Int64)(((Runtime.Number)arguments[0]).Value) << (int)((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Number(double.NaN);
    }

    [Function(Const.RightShiftFunc)]
    public static IObject RightShift(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Number(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Number(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Number((Int64)(((Runtime.Number)arguments[0]).Value) >> (int)((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Number(double.NaN);
    }

    [Function(Const.XOrFunc)]
    public static IObject Xor(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Number(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Number(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Number((Int64)(((Runtime.Number)arguments[0]).Value) ^ (Int64)((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Number(double.NaN);
    }

    [Function(Const.OrFunc)]
    public static IObject Or(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Number(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Number(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Number((Int64)(((Runtime.Number)arguments[0]).Value) | (Int64)((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Number(double.NaN);
    }

    [Function(Const.NotFunc)]
    public static IObject Not(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 1)
        {
            return new Runtime.Number(double.NaN);
        }

        return new Runtime.Number(~(Int64)(((Runtime.Number)arguments[0]).Value));
    }

    [Function(Const.AndFunc)]
    public static IObject And(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Number(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Number(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Number((Int64)(((Runtime.Number)arguments[0]).Value) & (Int64)((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Number(double.NaN);
    }

    [Function(Const.AddFunc)]
    public static IObject Add(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Number(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Number(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Number(((Runtime.Number)arguments[0]).Value + ((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Number(double.NaN);
    }
    public static IObject Add(IObject v1, IObject v2)
    {
        return new Runtime.Number(((Runtime.Number)v1).Value + ((Runtime.Number)v2).Value);
    }

    [Function(Const.SubFunc)]
    public static IObject Sub(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Number(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Number(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Number(((Runtime.Number)arguments[0]).Value - ((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Number(double.NaN);
    }

    [Function(Const.MulFunc)]
    public static IObject Mul(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Number(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Number(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Number(((Runtime.Number)arguments[0]).Value * ((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Number(double.NaN);
    }

    [Function(Const.ModFunc)]
    public static IObject Mod(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Number(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Number(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Number(((Runtime.Number)arguments[0]).Value % ((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Number(double.NaN);
    }

    [Function(Const.DivFunc)]
    public static IObject Div(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Number(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new Runtime.Number(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new Runtime.Number(((Runtime.Number)arguments[0]).Value / ((Runtime.Number)arguments[1]).Value);
        }

        return new Runtime.Number(double.NaN);
    }

    [Function(Const.CallFunc)]
    public static IObject Call(IObject[] arguments)
    {
        throw new Exception();
    }
}