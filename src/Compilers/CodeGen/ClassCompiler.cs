using Pain.Runtime;
namespace Pain.Compilers.CodeGen
{
    public class ClassCompiler
    {
        private readonly Strings _strings;

        private readonly ModuleContext _module;

        private readonly ClassContext _class;

        public ClassCompiler(ModuleContext module, ClassContext @class, Strings strings)
        {
            _class = @class;
            _module = module;
            _strings = strings;
        }

        public RuntimeClass Compile()
        {
            var functionTable = new Pain.Runtime.FunctionTable();
            foreach (var item in CompileFunctions())
            {
                functionTable.AddFunction(item.Name, item);
            }

            return new RuntimeClass(_class.Name, _class.Definition.Super, _class.Module.Path, functionTable);
        }

        public static RuntimeClass Compile(ModuleContext module, ClassContext @class, Strings strings)
        {
            return new ClassCompiler(module, @class, strings).Compile();
        }

        private IEnumerable<CompiledFunction> CompileFunctions()
        {
            foreach (var item in _class.Definition.Functions)
            {
                _class.CreateFunction(item);
            }

            foreach (var item in _class.Functions)
            {
                var compiledFunction = FunctionCompiler.CompileFunction(item.Value, _strings);
                if (compiledFunction == null)
                {
                    throw new Exception();
                }

                yield return compiledFunction;
            }
        }
    }
}