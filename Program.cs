using Pain.Runtime;
using Pain.Compilers.CodeGen;
using Pain.Runtime.VM;

namespace Pain
{
    public class Program
    {
        public static void Main()
        {
            var strings = new Strings();
            var classLoader = new ClassLoader(token => ModuleCompiler.Compile(token, strings));
            var vm = new VirtualMachine(classLoader, strings);
            vm.Execute("program", "Program", "main", Array.Empty<Pain.Runtime.IObject>());
        }
    }
}
