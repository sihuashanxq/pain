using Pain.Runtime.VM;
namespace Pain.Runtime
{
    public class Boolean : IObject
    {
        public static readonly Boolean True;

        public static readonly Boolean Flase;

        public readonly static BooleanType Type;

        public bool Value { get; private set; }

        static Boolean()
        {
            Type = new BooleanType(Const.Runtime, Const.Object, Util.ScanFunctions(typeof(Boolean)));
            True = new Boolean(true);
            Flase = new Boolean(false);
        }

        public Boolean(bool value)
        {
            Value = value;
        }

        public RuntimeClass GetClass()
        {
            return Builtin.Boolean.Class;
        }

        public bool ToBoolean(VirtualMachine vm)
        {
            return Value;
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value)
        {
            throw new NotImplementedException();
        }

        public static bool IsBoolean(IObject v)
        {
            return v is Runtime.Boolean;
        }

        [Function(Const.ToStringFunc)]
        public static IObject ToString(IObject[] arguments)
        {
            return new Runtime.String(arguments[0].ToString()!);
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

            if (IsBoolean(arguments[1]))
            {
                return new Runtime.Boolean(((Runtime.Boolean)(arguments[0])).Value && ((Runtime.Boolean)(arguments[1])).Value);
            }

            return new Runtime.Boolean(false);
        }
    }
}