using Pain.Runtime.VM;
namespace Pain.Runtime
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

        public IObject Call(VirtualMachine vm, IObject[] arguments)
        {
            return vm.Execute(this, new[] { Target }.Concat(arguments).ToArray())!;
        }

        public bool ToBoolean(VirtualMachine vm)
        {
            return true;
        }

        public RuntimeClass GetClass()
        {
            return Builtin.Func.Class;
        }

        public void SetField(VirtualMachine vm, IObject key, IObject value)
        {
            throw new NotImplementedException();
        }
    }
}