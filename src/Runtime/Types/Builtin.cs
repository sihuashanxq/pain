namespace Pain.Runtime.Types
{
    public static class Builtin
    {
        public static ModuleToken Null { get; }

        public static ModuleToken Array { get; }

        public static ModuleToken String { get; }

        public static ModuleToken Number { get; }

        public static ModuleToken Object { get; }

        public static ModuleToken Boolean { get; }

        public static ModuleToken Function { get; }

        public static Type NullType { get; }

        public static Type ArrayType { get; }

        public static Type StringType { get; }

        public static Type NumberType { get; }

        public static Type ObjectType { get; }

        public static Type BooleanType { get; }

        public static Type FunctionType { get; }

        static Builtin()
        {
            Null = new ModuleToken(Const.Runtime, Const.Null);
            Array = new ModuleToken(Const.Runtime, Const.Array);
            Object = new ModuleToken(Const.Runtime, Const.Object);
            Number = new ModuleToken(Const.Runtime, Const.Number);
            String = new ModuleToken(Const.Runtime, Const.String);
            Boolean = new ModuleToken(Const.Runtime, Const.Boolean);
            Function = new ModuleToken(Const.Runtime, Const.Function);
            NullType = new NullType(Util.ScanFunctions(typeof(Null)));
            ArrayType = new ArrayType(Util.ScanFunctions(typeof(Array)));
            ObjectType = new Type(Object, null, Util.ScanFunctions(typeof(Object)));
            NumberType = new NumberType(Util.ScanFunctions(typeof(Number)));
            StringType = new StringType(Util.ScanFunctions(typeof(String)));
            BooleanType = new BooleanType(Util.ScanFunctions(typeof(Boolean)));
            FunctionType = new Type(Function, Object, Util.ScanFunctions(typeof(Function)));
        }

        public static Type? GetType(ModuleToken token)
        {
            switch (token.ToString())
            {
                case $"{Const.Runtime}.{Const.Null}":
                    return NullType;
                case $"{Const.Runtime}.{Const.Array}":
                    return ArrayType;
                case $"{Const.Runtime}.{Const.Object}":
                    return ObjectType;
                case $"{Const.Runtime}.{Const.String}":
                    return StringType;
                case $"{Const.Runtime}.{Const.Number}":
                    return NumberType;
                case $"{Const.Runtime}.{Const.Boolean}":
                    return BooleanType;
                case $"{Const.Runtime}.{Const.Function}":
                    return FunctionType;

                default:
                    return null;
            }
        }
    }
}