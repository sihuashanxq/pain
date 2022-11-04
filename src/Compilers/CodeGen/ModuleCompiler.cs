using Pain.Compilers.Parsers.Definitions;
using Pain.Runtime;
namespace Pain.Compilers.CodeGen
{
    public class ModuleCompiler
    {
        private readonly Strings _strings;

        private readonly ModuleContext _module;

        private readonly ClassLoader _classLoader;

        public ModuleCompiler(ModuleDefinition module, ClassLoader classLoader, Strings strings)
        {
            _module = new ModuleContext(module);
            _strings = strings;
            _classLoader = classLoader;
        }

        public IEnumerable<RuntimeClass> Compile()
        {
            return _module.Classes.Select(item => ClassCompiler.Compile(_module, item.Value, _strings));
        }
    }
}