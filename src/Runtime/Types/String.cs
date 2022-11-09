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

        public Type GetType(VirtualMachine vm, out bool @throw)
        {
            @throw = false;
            return Builtin.StringType;
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value, out bool @throw)
        {
            @throw = false;
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

        [Function("len")]
        public static IObject Len(IObject[] args, out bool @throw)
        {
            @throw = false;
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException();
            }

            return new Number((args[0] as String).Value.Length);
        }

        [Function("getChar")]
        public static IObject GetChar(IObject[] args, out bool @throw)
        {
            @throw = false;
            if (args == null || args.Length < 2)
            {
                throw new ArgumentException();
            }

            return new String((args[0] as String).Value[(int)((args[1] as Number)).Value].ToString());
        }


        [Function(Const.ToStringFunc)]
        public static IObject ToString(IObject[] args, out bool @throw)
        {
            @throw = false;
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException();
            }

            return args[0];
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

            if (args[0] is String v1 && args[1] is String v2)
            {
                return v1.Value == v2.Value ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.AddFunc)]
        public static IObject Add(IObject[] args, out bool @throw)
        {
            @throw = false;
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
        public static IObject LessThan(IObject[] args, out bool @throw)
        {
            @throw = false;
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
        public static IObject GreaterThan(IObject[] args, out bool @throw)
        {
            @throw = false;
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
        public static IObject LessThanOrEqual(IObject[] args, out bool @throw)
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

            if (args[0] is String str1 && args[1] is String str2)
            {
                return System.String.CompareOrdinal(str1.Value, str2.Value) != 1 ? Boolean.True : Boolean.False;
            }

            return Boolean.False;
        }

        [Function(Const.GtreaterThanOrEqualFunc)]
        public static IObject GtreaterThanOrEqual(IObject[] args, out bool @throw)
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