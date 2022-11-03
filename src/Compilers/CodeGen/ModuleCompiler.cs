using Pain.Compilers.Parsers.Definitions;
using Pain.Runtime;
namespace Pain.Compilers.CodeGen
{
    public class ModuleCompiler
    {
        private readonly Strings _strings;

        private readonly ModuleDefinition _module;

        public ModuleCompiler(ModuleDefinition module, Strings strings)
        {
            _module = module;
            _strings = strings;
        }

        public IEnumerable<RuntimeClass> Compile()
        {
            var ctx = new ModuleContext(_module.Path);
            foreach (var module in _module.Imports)
            {
                foreach (var item in module.Classes)
                {
                    ctx.AddImporedClass(new ImportedClass(module.Path, item.Name, item.Alias))
                }
            }

            foreach (var item in _module.Classes)
            {
                ctx.AddClass(item);
            }

            foreach (var item in ctx.Classes)
            {
                var compiler = new ClassCompiler(ctx, item.Value, null, _strings);
                yield return compiler.Compile();
            }
        }
    }
}