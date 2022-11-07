using Pain.Compilers.Parsers.Definitions;
using Pain.Compilers.Parsers;
using RuntimeType = Pain.Runtime.Types.Type;
namespace Pain.Compilers.CodeGen
{
    public class ModuleCompiler
    {
        private readonly Strings _strings;

        private readonly ModuleContext _module;

        public ModuleCompiler(Module module, Strings strings)
        {
            _module = new ModuleContext(module);
            _strings = strings;
        }

        public IEnumerable<RuntimeType> Compile()
        {
            return _module.Classes.Select(item => ClassCompiler.Compile(_module, item.Value, _strings));
        }

        public static IEnumerable<RuntimeType> Compile(Module module, Strings strings)
        {
            return new ModuleCompiler(module, strings).Compile();
        }

        public static IEnumerable<RuntimeType> Compile(ModuleToken token, Strings strings)
        {
            var sourceCode = ModuleReader.Read(token);
            var module = Parser.Parse(token.Module, sourceCode)!;
            return Compile(module, strings);
        }
    }
}