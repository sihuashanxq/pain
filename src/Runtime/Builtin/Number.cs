namespace Pain.Runtime.Builtin;

[Class(Const.Runtime, Const.Number)]
public static class Number
{
    public static RuntimeNumberClass Class { get; }

    static Number()
    {
        Class = new RuntimeNumberClass(Const.Runtime, Const.Object, Util.ScanFunctions(typeof(Number)));
    }

    public static bool IsNumber(IObject v2)
    {
        return v2 is RuntimeNumber;
    }

    [Function(Const.ToStringFunc)]
    public static IObject ToString(IObject[] arguments)
    {
        if (arguments == null || arguments.Length == 0)
        {
            throw new ArgumentException();
        }

        return new RuntimeString(((RuntimeNumber)arguments[0]).Value.ToString());
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

        if (IsNumber(arguments[1]))
        {
            return new RuntimeBoolean(((RuntimeNumber)arguments[0]).Value == ((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeBoolean(false);
    }

    [Function(Const.LessThanFunc)]
    public static IObject LessThan(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeBoolean(false);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeBoolean(true);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeBoolean(((RuntimeNumber)arguments[0]).Value < ((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeBoolean(false);
    }

    [Function(Const.GreaterThanFunc)]
    public static IObject GreaterThan(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeBoolean(false);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeBoolean(true);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeBoolean(((RuntimeNumber)arguments[0]).Value > ((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeBoolean(false);
    }

    [Function(Const.LessThanOrEqualFunc)]
    public static IObject LessThanOrEqual(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeBoolean(false);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeBoolean(true);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeBoolean(((RuntimeNumber)arguments[0]).Value <= ((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeBoolean(false);
    }

    [Function(Const.GtreaterThanOrEqualFunc)]
    public static IObject GtreaterThanOrEqual(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeBoolean(false);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeBoolean(true);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeBoolean(((RuntimeNumber)arguments[0]).Value >= ((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeBoolean(false);
    }

    [Function(Const.LeftShiftFunc)]
    public static IObject LeftShfit(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeNumber(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeNumber(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeNumber((Int64)(((RuntimeNumber)arguments[0]).Value) << (int)((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeNumber(double.NaN);
    }

    [Function(Const.RightShiftFunc)]
    public static IObject RightShift(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeNumber(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeNumber(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeNumber((Int64)(((RuntimeNumber)arguments[0]).Value) >> (int)((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeNumber(double.NaN);
    }

    [Function(Const.XOrFunc)]
    public static IObject Xor(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeNumber(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeNumber(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeNumber((Int64)(((RuntimeNumber)arguments[0]).Value) ^ (Int64)((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeNumber(double.NaN);
    }

    [Function(Const.OrFunc)]
    public static IObject Or(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeNumber(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeNumber(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeNumber((Int64)(((RuntimeNumber)arguments[0]).Value) | (Int64)((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeNumber(double.NaN);
    }

    [Function(Const.NotFunc)]
    public static IObject Not(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 1)
        {
            return new RuntimeNumber(double.NaN);
        }

        return new RuntimeNumber(~(Int64)(((RuntimeNumber)arguments[0]).Value));
    }

    [Function(Const.AndFunc)]
    public static IObject And(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeNumber(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeNumber(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeNumber((Int64)(((RuntimeNumber)arguments[0]).Value) & (Int64)((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeNumber(double.NaN);
    }

    [Function(Const.AddFunc)]
    public static IObject Add(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeNumber(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeNumber(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeNumber(((RuntimeNumber)arguments[0]).Value + ((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeNumber(double.NaN);
    }

    [Function(Const.SubFunc)]
    public static IObject Sub(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeNumber(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeNumber(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeNumber(((RuntimeNumber)arguments[0]).Value - ((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeNumber(double.NaN);
    }

    [Function(Const.MulFunc)]
    public static IObject Mul(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeNumber(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeNumber(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeNumber(((RuntimeNumber)arguments[0]).Value * ((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeNumber(double.NaN);
    }

    [Function(Const.ModFunc)]
    public static IObject Mod(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeNumber(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeNumber(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeNumber(((RuntimeNumber)arguments[0]).Value % ((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeNumber(double.NaN);
    }

    [Function(Const.DivFunc)]
    public static IObject Div(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new RuntimeNumber(double.NaN);
        }

        if (arguments[0] == arguments[1])
        {
            return new RuntimeNumber(double.NaN);
        }

        if (IsNumber(arguments[1]))
        {
            return new RuntimeNumber(((RuntimeNumber)arguments[0]).Value / ((RuntimeNumber)arguments[1]).Value);
        }

        return new RuntimeNumber(double.NaN);
    }

    [Function(Const.CallFunc)]
    public static IObject Call(IObject[] arguments)
    {
        throw new Exception();
    }
}