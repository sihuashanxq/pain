namespace Pain.Runtime;
using System.Reflection;
public class Function
{
    public bool Native { get; }

    public string Name { get; }

    public MethodInfo Method { get; }

    public byte[] OpCodes { get; }

    public int MaxStackSize { get; }

    public int ParameterCount { get; }

    public Function(string name, bool natvie, byte[] opcodes, int maxStackSize, int parameterCount, MethodInfo methodInfo)
    {
        Name = name;
        OpCodes = opcodes;
        Native=natvie;
        Method = methodInfo;
        MaxStackSize = maxStackSize;
        ParameterCount = parameterCount;
    }
}