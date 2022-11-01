namespace Pain.Runtime.Metadata;

public class MetadataType
{
    public string Name { get; }
    
    public string Module { get; }

    public MetadataType Super { get; }


    public FunctionTable FunctionTable { get; }

    public MetadataType(MetadataType super, string name, string module, FunctionTable functionTable)
    {
        Name = name;
        Super = super;
        Module = module;
        FunctionTable = functionTable;
    }
}

public class Function
{
    public bool Local { get; }

    public byte[] OpCodes { get; }

    public int MaxStackSize { get; }

    public MetadataType Type { get; }

    public int ParameterCount { get; }

    public Function(MetadataType type, bool @local, byte[] opcodes, int maxStackSize, int parameterCount)
    {
        Type = type;
        Local = local;
        OpCodes = opcodes;
        MaxStackSize = maxStackSize;
        ParameterCount = parameterCount;
    }
}

public class FunctionTable
{
    public Dictionary<string, Function> Functions { get; }

    public FunctionTable()
    {
        Functions = new Dictionary<string, Function>();
    }

    public Function GetFunction(string name)
    {
        return Functions[name];
    }
}