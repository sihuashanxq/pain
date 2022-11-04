namespace Pain.Runtime.Builtin;

[Class(Const.Runtime, Const.Array)]
public static class Array
{
    public static RuntimeArrayClass Class { get; }

    static Array()
    {
        Class = new RuntimeArrayClass(Const.Runtime, Const.Object, Util.ScanFunctions(typeof(Array)));
    }

    public static bool IsArray(IObject v)
    {
        return v is RuntimeArray;
    }

    [Function("len")]
    public static IObject Len(IObject[] arguments){
        return new RuntimeNumber( (arguments[0] as RuntimeArray).Items.Length!);
    }

    [Function(Const.ToStringFunc)]
    public static IObject ToString(IObject[] arguments)
    {
        return new RuntimeString($"[{string.Join(", ", arguments.Select(i => i.ToString()))}]");
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
}