namespace Pain.Runtime.Builtin;

[Class(Const.Runtime, Const.String)]
public static class String
{
    public static RuntimeStringClass Class { get; }

    static String()
    {
        Class = new RuntimeStringClass(Const.Runtime, Const.Object, Util.ScanFunctions(typeof(String)));
    }

    public static bool IsString(IObject v)
    {
        return v is Runtime.String;
    }

    [Function(Const.ToStringFunc)]
    public static IObject ToString(IObject[] arguments)
    {
        if (arguments == null || arguments.Length == 0)
        {
            throw new ArgumentException();
        }

        return arguments[0];
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

        if (IsString(arguments[1]))
        {
            return new Runtime.Boolean(((Runtime.String)arguments[0]).Value == ((Runtime.String)(arguments[1])).Value);
        }

        return new Runtime.Boolean(false);
    }

    [Function(Const.AddFunc)]
    public static IObject Add(IObject[] arguments)
    {
        if (arguments == null || arguments.Length != 2)
        {
            return new Runtime.Boolean(false);
        }

        return new Runtime.String(arguments[0].ToString() + arguments[1].ToString());
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

        if (IsString(arguments[1]))
        {
            return new Runtime.Boolean(System.String.CompareOrdinal(((Runtime.String)arguments[0]).Value, ((Runtime.String)(arguments[1])).Value) == -1);
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

        if (IsString(arguments[1]))
        {
            return new Runtime.Boolean(System.String.CompareOrdinal(((Runtime.String)arguments[0]).Value, ((Runtime.String)(arguments[1])).Value) == 1);
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

        if (IsString(arguments[1]))
        {
            return new Runtime.Boolean(System.String.CompareOrdinal(((Runtime.String)arguments[0]).Value, ((Runtime.String)(arguments[1])).Value) != 1);
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

        if (IsString(arguments[1]))
        {
            return new Runtime.Boolean(System.String.CompareOrdinal(((Runtime.String)arguments[0]).Value, ((Runtime.String)(arguments[1])).Value) != -1);
        }

        return new Runtime.Boolean(false);
    }
}