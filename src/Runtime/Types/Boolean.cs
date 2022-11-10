using Pain.Runtime.VM;
namespace Pain.Runtime.Types
{
    public class Boolean : IObject
    {
        public static ModuleToken Token { get; }

        public static Boolean True { get; }

        public static Boolean False { get; }

        public static BooleanType Type { get; }

        public bool Value { get; private set; }

        static Boolean()
        {
            Token = new ModuleToken(Const.Runtime, Const.Boolean);
            Type = new BooleanType(Util.ScanFunctions(typeof(Boolean)));
            True = new Boolean(true);
            False = new Boolean(false);
        }

        public Boolean(bool value)
        {
            Value = value;
        }

        public Type GetType(VirtualMachine vm, out bool @throw)
        {
            @throw = false;
            return Builtin.BooleanType;
        }

        public bool ToBoolean(VirtualMachine vm)
        {
            return Value;
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value, out bool @throw)
        {
            @throw = false;
            throw new NotImplementedException();
        }

        [Function(Const.ToStringFunc)]
        public static IObject ToString(IObject[] args, out bool @throw)
        {
            @throw = false;
            return new String(args[0].ToString()!);
        }

        [Function(Const.EqualFunc)]
        public static IObject Euqal(IObject[] args, out bool @throw)
        {
            @throw = false;
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] == args[1])
            {
                return Boolean.True;
            }

            if (args[0] is Boolean v1 && args[1] is Boolean v2)
            {
                return v1.Value == v2.Value ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }
    }


    public class BooleanType : Type
    {
        public BooleanType(FunctionTable table) : base(Builtin.Boolean, Builtin.Object, table)
        {

        }

        public override IObject CreateInstance()
        {
            return new Boolean(false);
        }
    }
}