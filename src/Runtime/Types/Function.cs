using Pain.Runtime.VM;
namespace Pain.Runtime.Types
{
    public class Function : IObject
    {
        public IObject Target { get; }

        public CompiledFunction Func { get; }

        public Function(IObject target, CompiledFunction func)
        {
            Func = func;
            Target = target;
        }

        public IObject Call(VirtualMachine vm, IObject[] arguments, out bool @throw)
        {
            return vm.Execute(this, new[] { Target }.Concat(arguments).ToArray(), out @throw)!;
        }

        public bool ToBoolean(VirtualMachine vm)
        {
            return true;
        }

        public Type GetType(VirtualMachine vm, out bool @throw)
        {
            @throw = false;
            return Builtin.FunctionType;
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value, out bool @throw)
        {
            @throw = false;
            throw new NotImplementedException();
        }
    }
}