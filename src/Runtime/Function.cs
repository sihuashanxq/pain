namespace Pain.Runtime;
using System.Reflection;
public class Function
{
    public bool Native { get; }

    public string Name { get; }

    public byte[] OpCodes { get; }

    public int MaxStackSize { get; }

    public int ParameterCount { get; }

    public Func<IObject[], IObject> Delegate { get; }

    public Function(string name, bool natvie, byte[] opcodes, int maxStackSize, int parameterCount, MethodInfo methodInfo)
    {
        Name = name;
        OpCodes = opcodes;
        Native = natvie;
        MaxStackSize = maxStackSize;
        ParameterCount = parameterCount;
        if (methodInfo != null)
        {
            Delegate = (Func<IObject[], IObject>)methodInfo.CreateDelegate(typeof(Func<IObject[], IObject>));
        }
    }
}