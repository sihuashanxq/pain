namespace Pain.Runtime;
using System.Reflection;
public class CompiledFunction
{
    public bool Native { get; }

    public string Name { get; }

    public byte[] OpCodes { get; }

    public int MaxSlotSize { get; }

    public int ParameterCount { get; }

    public Func<IObject[], IObject>? Delegate { get; }

    public CompiledFunction(string name, bool natvie, byte[] opcodes, int maxSlotSize ,int parameterCount, MethodInfo methodInfo)
    {
        Name = name;
        OpCodes = opcodes;
        Native = natvie;
        MaxSlotSize = maxSlotSize;
        ParameterCount = parameterCount;
        if (methodInfo != null)
        {
            Delegate = (Func<IObject[], IObject>)methodInfo.CreateDelegate(typeof(Func<IObject[], IObject>));
        }
    }
}