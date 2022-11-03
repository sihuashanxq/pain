using Pain.Runtime;
namespace Pain.Compilers.CodeGen
{
    public class ClassCompiler
    {
        private readonly Strings _strings;

        private readonly RuntimeClass _super;

        private readonly ModuleContext _module;

        private readonly ClassContext _class;

        public ClassCompiler(ModuleContext module, ClassContext @class, RuntimeClass super, Strings strings)
        {
            _class = @class;
            _super = super;
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

            return new RuntimeClass(_super, _class.Name, _class.Module.Path, functionTable);
        }

        private IEnumerable<Function> CompileFunctions()
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