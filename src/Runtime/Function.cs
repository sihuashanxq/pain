namespace Pain.Runtime;

public class Function
{
    public bool Local { get; }

    public string Name { get; }

    public byte[] OpCodes { get; }
    
    public int MaxStackSize { get; }

    public ClassObject Class { get; }

    public int ParameterCount { get; }


    public Function(ClassObject @class, string name, bool @local, byte[] opcodes, int maxStackSize, int parameterCount)
    {
        Name = name;
        Local = local;
        Class = @class;
        OpCodes = opcodes;
        MaxStackSize = maxStackSize;
        ParameterCount = parameterCount;
    }
}