namespace Pain.Runtime.Builtin
{
    [Class(Const.Runtime, Const.Function)]
    public static class Func
    {
        public static RuntimeClass Class { get; }

        static Func()
        {
            Class = new RuntimeClass(Const.Function, Object.Token, Const.Runtime, Util.ScanFunctions(typeof(Func)));
        }

        [Function("bind")]
        public static IObject ToString(IObject[] arguments)
        {
            return new Function(arguments[1], (arguments[0] as Function).Func);
        }
    }
}