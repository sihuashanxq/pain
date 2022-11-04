using Pain.Compilers.Parsers.Definitions;
using Pain.Runtime;
namespace Pain.Compilers.CodeGen
{
    public class ModuleCompiler
    {
        private readonly Strings _strings;

        public static Strings Strings;

        private readonly ModuleDefinition _module;

        private readonly ClassLoader _classLoader;

        public ModuleCompiler(ModuleDefinition module, ClassLoader classLoader, Strings strings)
        {
            _module = module;
            _strings = strings;
            Strings = strings;
            _classLoader = classLoader;
        }

        public IEnumerable<RuntimeClass> Compile()
        {
            var ctx = new ModuleContext(_module.Path);
            foreach (var module in _module.Imports)
            {
                foreach (var item in module.Classes)
                {
                    ctx.AddImporedClass(new ImportedClass(module.Path, item.Name, item.Alias));
                }
            }

            foreach (var item in _module.Classes)
            {
                ctx.AddClass(item);
            }

            var hash = new HashSet<object>();

            foreach (var item in ctx.Classes)
            {
                if (hash.Contains(item))
                {
                    continue;
                }
                hash.Add(item);
                if (_module.Classes.FirstOrDefault(i => i.Name == item.Value.Definition.Super) != null)
                {

                    var compiler = new ClassCompiler(ctx, item.Value, _classLoader.Load(_module.Path + "." + item.Value.Definition.Super), _strings);
                    yield return compiler.Compile();
                }
                else
                {
                    var compiler = new ClassCompiler(ctx, item.Value, _classLoader.Load(item.Value.Definition.Super), _strings);
                    yield return compiler.Compile();
                }
            }

            foreach (var item in ctx.Classes)
            {
                if (hash.Contains(item))
                {
                    continue;
                }
                hash.Add(item);
                if (_module.Classes.FirstOrDefault(i => i.Name == item.Value.Definition.Super) != null)
                {

                    var compiler = new ClassCompiler(ctx, item.Value, _classLoader.Load(_module.Path + "." + item.Value.Definition.Super), _strings);
                    yield return compiler.Compile();
                }
                else
                {
                    var compiler = new ClassCompiler(ctx, item.Value, _classLoader.Load(item.Value.Definition.Super), _strings);
                    yield return compiler.Compile();
                }
            }
        }
    }
}