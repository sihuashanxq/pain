namespace Pain.Compilers.CodeGen;
using Pain.Compilers.Expressions;
using Pain.Compilers.Parsers.Definitions;
public class FunctionContext
{
    public string Name { get; }

    public bool Static { get; }

    public ClassContext Class { get; }

    public FunctionFrame Frame { get; }

    public FunctionExpression Expression { get; }

    public FunctionContext(FunctionExpression expression, ClassContext @class)
    {
        Name = expression.Name;
        Class = @class;
        Frame = new FunctionFrame();
        Expression = expression;
    }
}

public class ClassContext
{
    public string Name { get; }

    public string Token { get; }

    public ModuleContext Module { get; }

    public ClassDefinition Definition { get; }

    public Dictionary<string, FunctionContext> Functions { get; }

    internal ClassContext(ClassDefinition definition, ModuleContext module)
    {
        Name = definition.Name;
        Token = $"{module.Path}.{definition.Name}";
        Module = module;
        Definition = definition;
        Functions = new Dictionary<string, FunctionContext>();
    }

    public FunctionContext CreateFunction(FunctionExpression function)
    {
        if (Functions.ContainsKey(function.Name))
        {
            throw new System.Exception($"method {function.Name} has defined!");
        }

        return Functions[function.Name] = new FunctionContext(expression, this);
    }
}

public class ModuleContext
{
    public string Path { get; }

    public Dictionary<string, ClassContext> Classes { get; }

    public Dictionary<string, ImportedClass> ImportedClasses { get; }

    internal ModuleContext(string path)
    {
        Path = path;
        Classes = new Dictionary<string, ClassContext>();
        ImportedClasses = new Dictionary<string, ImportedClass>();
    }

    public ClassContext AddClass(Pain.Compilers.Parsers.Definitions.ClassDefinition @class)
    {
        if (Classes.ContainsKey(@class.Name))
        {
            throw new System.Exception($"type :{@class.Name} has defined!");
        }

        return Classes[@class.Name] = new ClassContext(@class, this);
    }

    public void AddImporedClass(ImportedClass importedClass)
    {
        ImportedClasses[importedClass.Alias] = importedClass;
    }
}

public class ImportedClass
{
    public string Name { get; }
    public string Token { get; }

    public string Alias { get; }

    public string Module { get; }


    public ImportedClass(string module, string name, string alias)
    {
        Name = name;
        Alias = alias;
        Token = $"{module}.{name}";
        Module = module;
    }
}