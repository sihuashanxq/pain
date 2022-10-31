namespace Pain.Runtime.Metadata;

public class Type
{
    public Type Super { get; }

    public string Name { get; }

    public string Module { get; }

    public Dictionary<string, Function> Functions { get; }
}

public class Function
{
    public Type Type { get; }

    public bool Local { get; }

    public byte[] OpCodes { get; }

    public int MaxStackSize { get; }

    public int ParameterCount { get; }
}

public class RuntimeType
{
    public Type Type { get; }
}

public class RuntimeFunctionTable
{
    public Object Target { get; }
    
    public RuntimeFunctionTable Super { get; }

    public Dictionary<string, Function> Functions { get; }
}

public class RuntimeFunction
{
    public Object Target { get; }

    public Function Function { get; }
}