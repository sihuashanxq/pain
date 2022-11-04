namespace Pain.Runtime.Builtin
{
    public static class Console
    {
        public static RuntimeClass Class { get; }

        static Console()
        {
            Class = new RuntimeClass("Console", Object.Token, Const.Runtime, Util.ScanFunctions(typeof(Console)));
        }

        [Function("log")]
        public static IObject Log(IObject[] arguments)
        {
            System.Console.WriteLine(arguments[1].ToString());
            return RuntimeNull.Null;
        }
    }
}