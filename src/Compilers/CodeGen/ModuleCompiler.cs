using Pain.Compilers.Parsers.Definitions;
using Pain.Runtime;
using Pain.Compilers.Parsers;
namespace Pain.Compilers.CodeGen
{
    public class ModuleCompiler
    {
        private readonly Strings _strings;

        private readonly ModuleContext _module;

        public ModuleCompiler(ModuleDefinition module, Strings strings)
        {
            _module = new ModuleContext(module);
            _strings = strings;
        }

        public IEnumerable<RuntimeClass> Compile()
        {
            return _module.Classes.Select(item => ClassCompiler.Compile(_module, item.Value, _strings));
        }

        public static IEnumerable<RuntimeClass> Compile(ModuleDefinition module, Strings strings)
        {
            return new ModuleCompiler(module, strings).Compile();
        }

        public static IEnumerable<RuntimeClass> Compile(string token, Strings strings)
        {
            var sourceCode = ModuleReader.Read(token);
            var module = Parser.Parse(token.Substring(0, token.LastIndexOf(".")), sourceCode)!;
            return Compile(module, strings);
        }
    }
}