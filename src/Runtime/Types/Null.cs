using Pain.Runtime.VM;
namespace Pain.Runtime
{
    public class Null : IObject
    {
        public static readonly IObject Const = new Null();

        public RuntimeClass GetClass()
        {
            return Builtin.Null.Class;
        }

        public IObject GetField(VirtualMachine vm, IObject key)
        {
            return GetClass().GetFunction(vm, this, key);
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value)
        {
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
    }
}