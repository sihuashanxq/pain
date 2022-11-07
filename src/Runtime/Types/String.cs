using Pain.Runtime.VM;
namespace Pain.Runtime
{
    public class String : IObject
    {
        public string Value { get; internal set; }

        public String(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public bool ToBoolean(VirtualMachine vm)
        {
            return !string.IsNullOrEmpty(Value);
        }

        public RuntimeClass GetClass()
        {
            return Builtin.String.Class;
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is String v)
            {
                return v.Value == Value;
            }

            return false;
        }
    }
}