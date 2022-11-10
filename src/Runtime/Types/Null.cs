using Pain.Runtime.VM;
namespace Pain.Runtime.Types
{
    public class Null : IObject
    {
        public static readonly IObject Value = new Null();

        public Type GetType(VirtualMachine vm, out bool @throw)
        {
            @throw = false;
            return Builtin.NullType;
        }

        public IObject GetField(VirtualMachine vm, IObject key, out bool @throw)
        {
            return GetType(vm, out @throw).GetFunction(vm, this, key)!;
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value, out bool @throw)
        {
            @throw = false;
            throw new NotImplementedException();
        }

        public bool ToBoolean(VirtualMachine vm)
        {
            return false;
        }

        public override string ToString()
        {
            return "null";
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

            if (args[0] == args[1] || args[1] is Null)
            {
                return Boolean.True;
            }

            return Boolean.False;
        }
    }
}