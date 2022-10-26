namespace Pain.Compilers.CodeGen;

public class FunctionContext
{
    public string Name { get; }

    public bool Static { get; }

    public TypeContext Type { get; }

    public FunctionFrame Frame { get; }

    public Dictionary<string, FunctionContext> Functions { get; }

    public FunctionContext(string name, TypeContext type)
    {
        Name = name;
        Type = type;
        Frame = new FunctionFrame();
        Functions = new Dictionary<string, FunctionContext>();
    }
}

public class TypeContext
{
    public string Name { get; }

    public ModuleContext Module { get; }

    public Dictionary<string, FunctionContext> Functions { get; }

    internal TypeContext(string name, ModuleContext module)
    {
        Name = name;
        Module = module;
        Functions = new Dictionary<string, FunctionContext>();
    }

    public FunctionContext CreateFunction(string name)
    {
        if (Functions.ContainsKey(name))
        {
            throw new System.Exception($"method {name} has defined!");
        }

        return Functions[name] = new FunctionContext(name, this);
    }
}
public class ModuleContext
{
    public string Path { get; }

    public ConstantPool<string> Strings;

    public ConstantPool<double> Numbers;

    public Dictionary<string, TypeContext> Types { get; }

    internal ModuleContext(string path)
    {
        Path = path;
        Strings = new ConstantPool<string>();
        Numbers = new ConstantPool<double>();
        Types = new Dictionary<string, TypeContext>();
    }

    public int Pooling(string v)
    {
        throw new Exception();
    }

    public int Pooling(double v)
    {
        throw new Exception();
    }

    public TypeContext DefineType(string name)
    {
        if (Types.ContainsKey(name))
        {
            throw new System.Exception($"type :{name} has defined!");
        }

        return Types[name] = new TypeContext(name, this);
    }
}