using Pain.Runtime;
using RuntimeType = Pain.Runtime.Types.Type;
using Pain.Runtime.Types;
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

        public RuntimeType Compile()
        {
            var table = new FunctionTable();
            foreach (var item in CompileFunctions())
            {
                table.Add(item.Name, item);
            }

            var builtin = Runtime.Types.Builtin.GetType(_class.Token);
            if (builtin != null)
            {
                builtin.FunctionTable.Add(table);
                return builtin;
            }

            return new RuntimeType(_class.Token, _class.Super, table);
        }

        public static RuntimeType Compile(ModuleContext module, ClassContext @class, Strings strings)
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