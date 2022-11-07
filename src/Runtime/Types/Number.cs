using Pain.Runtime.VM;
namespace Pain.Runtime
{
    public class Number : IObject
    {
        public double Value { get; internal set; }

        public Number(double value)
        {
            Value = value;
        }

        public bool ToBoolean(VirtualMachine vm)
        {
            return Value != 0;
        }

        public RuntimeClass GetClass()
        {
            return Builtin.Number.Class;
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value)
        {
            throw new Exception();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}