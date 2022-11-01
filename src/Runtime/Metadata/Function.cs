namespace Pain.Runtime.Metadata;

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