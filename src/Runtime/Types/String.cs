using Pain.Runtime.VM;
namespace Pain.Runtime.Types
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

        public Type GetType(VirtualMachine vm)
        {
            return Builtin.StringType;
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

        [Function(Const.ToStringFunc)]
        public static IObject ToString(IObject[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
            {
                throw new ArgumentException();
            }

            return arguments[0];
        }

        [Function(Const.EqualFunc)]
        public static IObject Euqal(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] == args[1])
            {
                return Boolean.True;
            }

            if (args[0] is String v1 && args[1] is String v2)
            {
                return v1.Value == v2.Value ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.AddFunc)]
        public static IObject Add(IObject[] args)
        {
            if (args == null || args.Length == 0)
            {
                return Null.Value;
            }

            if (args.Length != 2)
            {
                return args[0];
            }

            if (args[0] is String str1 && args[1] is String str2)
            {
                return new String(str1.Value + str2.Value);
            }

            return Null.Value;
        }

        [Function(Const.LessThanFunc)]
        public static IObject LessThan(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] == args[1])
            {
                return Boolean.False;
            }

            if (args[0] is String str1 && args[1] is String str2)
            {
                return System.String.CompareOrdinal(str1.Value, str2.Value) == -1 ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.GreaterThanFunc)]
        public static IObject GreaterThan(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] == args[1])
            {
                return Boolean.False;
            }

            if (args[0] == args[1])
            {
                return Boolean.True;
            }

            if (args[0] is String str1 && args[1] is String str2)
            {
                return System.String.CompareOrdinal(str1.Value, str2.Value) == 1 ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.LessThanOrEqualFunc)]
        public static IObject LessThanOrEqual(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] == args[1])
            {
                return Boolean.True;
            }

            if (args[0] is String str1 && args[1] is String str2)
            {
                return System.String.CompareOrdinal(str1.Value, str2.Value) != 1 ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.GtreaterThanOrEqualFunc)]
        public static IObject GtreaterThanOrEqual(IObject[] args)
        {
            if (args == null || args.Length != 2)
            {
                return Boolean.False;
            }

            if (args[0] == args[1])
            {
                return Boolean.True;
            }

            if (args[0] is String str1 && args[1] is String str2)
            {
                return System.String.CompareOrdinal(str1.Value, str2.Value) != -1 ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }
    }

    public class StringType : Type
    {
        public StringType(FunctionTable table) : base(Builtin.String, Builtin.Object, table)
        {

        }

        public override IObject CreateInstance()
        {
            return new String(string.Empty);
        }
    }

}