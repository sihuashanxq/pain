using Pain.Runtime.VM;
namespace Pain.Runtime.Types
{
    public class Type : IObject
    {
        private Type? _super;

        public ModuleToken Token { get; }

        public ModuleToken? Super { get; }

        public FunctionTable FunctionTable { get; }

        public Type(ModuleToken token, ModuleToken? super, FunctionTable functionTable)
        {
            Token = token;
            Super = super;
            FunctionTable = functionTable;
        }

        public Function? GetFunction(VirtualMachine vm, IObject target, IObject name)
        {
            if (FunctionTable.TryGet(name.ToString()!, out var function))
            {
                return new Function(target, function!);
            }

            if (_super == null)
            {
                if (Super != null)
                {
                    _super = vm.GetClassLoader().Load(Super);
                }
            }

            return _super?.GetFunction(vm, target, name);
        }

        public virtual IObject CreateInstance()
        {
            return new Object(this);
        }

        public bool ToBoolean(VirtualMachine vm)
        {
            return true;
        }

        public Type GetType(VirtualMachine vm)
        {
            return this;
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value)
        {
            throw new NotImplementedException();
        }
    }

    public class NullType : Type
    {
        public NullType(FunctionTable table) : base(Builtin.Null, Builtin.Object, table)
        {

        }

        public override IObject CreateInstance()
        {
            return new Boolean(false);
        }
    }

    public class NumberType : Type
    {
        public NumberType(FunctionTable table) : base(Builtin.Number, Builtin.Object, table)
        {

        }

        public override IObject CreateInstance()
        {
            return new Number(0d);
        }
    }
}