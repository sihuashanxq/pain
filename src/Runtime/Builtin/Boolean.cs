namespace Pain.Runtime.Builtin;

[Class(Const.Runtime, Const.String)]
public static class Boolean
{
    public static RuntimeBooleanClass Class { get; }

    static Boolean()
    {
        Class = new RuntimeBooleanClass(Const.Runtime, Const.Object, Util.ScanFunctions(typeof(Boolean)));
    }

    public static bool IsBoolean(IObject v)
    {
        return v is RuntimeBoolean;
    }

    [Function(Const.ToStringFunc)]
    public static IObject ToString(IObject[] arguments)
    {
        return new RuntimeString(arguments[0].ToString()!);
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

        if (IsBoolean(arguments[1]))
        {
            return new RuntimeBoolean(((RuntimeBoolean)(arguments[0])).Value && ((RuntimeBoolean)(arguments[1])).Value);
        }

        return new RuntimeBoolean(false);
    }
}