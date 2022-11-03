namespace Pain.Runtime.Builtin
{
    [Class(Const.Runtime, Const.Null)]
    public class Null
    {
        public static RuntimeNullClass Class { get; }

        static Null()
        {
            Class = new RuntimeNullClass(Const.Runtime, Const.Object, Util.ScanFunctions(typeof(Object)));
        }

        [Function(Const.ToStringFunc)]
        public static IObject ToString(IObject[] arguments)
        {
            return new RuntimeString("null");
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

            return new RuntimeBoolean(arguments[1] is RuntimeNull);
        }
    }
}