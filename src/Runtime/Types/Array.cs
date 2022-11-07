using Pain.Runtime.VM;
namespace Pain.Runtime.Types
{
    public class Array : IObject
    {
        public static Type Type { get; }

        public static ModuleToken Token { get; }

        public IObject[] Items { get; private set; }

        static Array()
        {
            Token = new ModuleToken(Const.Runtime, Const.Array);
            Type = new ArrayType(Util.ScanFunctions(typeof(Array)));
        }

        public Array()
        {
            Items = new IObject[0];
        }

        public bool ToBoolean(VirtualMachine vm)
        {
            return true;
        }

        public Type GetType(VirtualMachine vm)
        {
            return Type;
        }

        public IObject GetField(VirtualMachine vm, IObject index)
        {
            if (index is String str)
            {
                return GetType(vm).GetFunction(vm, this, index)!;
            }

            if (index is Number idx)
            {
                var i = idx.Value;
                if (i < 0 || i >= Items.Length)
                {
                    return Null.Value;
                }

                return Items[(int)i] ?? Null.Value;
            }

            return Null.Value;
        }

        public void SetField(VirtualMachine vm, IObject index, IObject value)
        {
            if (index is Number idx)
            {
                var i = (int)idx.Value;
                if (i < 0 || i >= Items.Length)
                {
                    var items = new IObject[(int)i + 1];
                    System.Array.Copy(Items, items, Items.Length);
                    Items = items;
                }

                Items[i] = value;
            }
        }

        public static bool IsArray(IObject v)
        {
            return v is Array;
        }

        [Function("len")]
        public static IObject Len(IObject[] args)
        {
            return new Number((args[0] as Array).Items.Length!);
        }

        [Function(Const.ToStringFunc)]
        public static IObject ToString(IObject[] args)
        {
            return new String($"[{string.Join(", ", (args[0] as Array)?.Items.Select((object i) => i.ToString()))}]");
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

            return Boolean.False;
        }
    }


    public class ArrayType : Type
    {
        public ArrayType(FunctionTable table) : base(Array.Token, Builtin.Object, table)
        {

        }

        public override IObject CreateInstance()
        {
            return new Array();
        }
    }
}