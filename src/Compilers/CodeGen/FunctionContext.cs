namespace Pain.Compilers.CodeGen;
using Pain.Compilers.Expressions;
using Pain.Compilers.Parsers.Definitions;
public class FunctionContext
{
    public string Name { get; }

    public bool Native { get; }

    public ClassContext Class { get; }

    public FunctionFrame Frame { get; }

    public FunctionExpression Expression { get; }

    public FunctionContext(FunctionExpression expression, ClassContext @class)
    {
        Name = expression.Name;
        Class = @class;
        Native = expression.Native;
        Frame = new FunctionFrame();
        Expression = expression;
    }
}

public class ClassContext
{
    public string Name { get; }

    public ModuleToken Token { get; }

    public ModuleToken Super { get; }

    public ModuleContext Module { get; }

    public Class Definition { get; }

    public Dictionary<string, FunctionContext> Functions { get; }

    internal ClassContext(Class definition, ModuleToken super, ModuleContext module)
    {
        Name = definition.Name;
        Token = new ModuleToken(module.Path, definition.Name);
        Module = module;
        Super = super;
        Definition = definition;
        Functions = new Dictionary<string, FunctionContext>();
    }

    public FunctionContext CreateFunction(FunctionExpression function)
    {
        if (Functions.ContainsKey(function.Name))
        {
            throw new System.Exception($"method {function.Name} has defined!");
        }

        return Functions[function.Name] = new FunctionContext(function, this);
    }
}

public class ModuleContext
{
    public string Path { get; }

    public Dictionary<string, ClassContext> Classes { get; }

    public Dictionary<string, Import> Imports { get; }

    internal ModuleContext(Module module)
    {
        Path = module.Path;
        Classes = module.Classes.ToDictionary(i => i.Key, i =>
        {
            if (module.Classes.ContainsKey(i.Value.Super))
            {
                var super = new ModuleToken(Path, i.Value.Super, i.Value.Super);
                return new ClassContext(i.Value, super, this);
            }

            if (module.Imports.TryGetValue(i.Value.Super, out var item))
            {
                var super = new ModuleToken(item.Module, item.Name, i.Value.Super);
                return new ClassContext(i.Value, super, this);
            }

            throw new Exception($"super was not found {i.Value.Super}");
        });

        Imports = module.Imports.ToDictionary(i => i.Key, i => i.Value);
    }
}

public class ImportedClass
{
    public string Name { get; }

    public ModuleToken Token { get; }

    public string Alias { get; }

    public string Module { get; }

    public ImportedClass(string module, string name, string alias)
    {
        Name = name;
        Alias = alias;
        Token = new ModuleToken(module, name);
        Module = module;
    }
}