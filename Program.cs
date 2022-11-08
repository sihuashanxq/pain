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
            classLoader.Load(new ModuleToken(Runtime.Types.Const.Runtime, Runtime.Types.Const.String));
            Console.WriteLine(vm.Execute(new ModuleToken("program", "Program"), "main", Array.Empty<Pain.Runtime.Types.IObject>()));
            Console.WriteLine(vm.Execute(new ModuleToken("program", "Program"), "main", Array.Empty<Pain.Runtime.Types.IObject>()));
        }
    }
}
