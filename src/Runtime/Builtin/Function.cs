namespace Pain.Runtime.Builtin
{
    [Class(Const.Runtime, Const.Function)]
    public static class FunctionBuiltin
    {
        public static RuntimeClass Class { get; }

        static FunctionBuiltin()
        {
            Class = new RuntimeClass(Object.Class, Const.Runtime, Const.Function, Util.ScanFunctions(typeof(FunctionBuiltin)));
        }

        [Function("bind")]
        public static IObject ToString(IObject[] arguments)
        {
            return new RuntimeFunction(arguments[1], (arguments[0] as RuntimeFunction).Function);
        }
    }
}