namespace Pain.Runtime.Builtin
{
    [Class(Const.Runtime, Const.Null)]
    public class Null
    {
        public static NullClass Class { get; }

        static Null()
        {
            Class = new NullClass(Const.Runtime, Const.Object, Util.ScanFunctions(typeof(Object)));
        }

        [Function(Const.ToStringFunc)]
        public static IObject ToString(IObject[] arguments)
        {
            return new Runtime.String("null");
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

            return new Runtime.Boolean(arguments[1] is Null);
        }
    }
}