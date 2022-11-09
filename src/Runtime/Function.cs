namespace Pain.Runtime;
using System.Reflection;
using Pain.Runtime.Types;
public class CompiledFunction
{
    public bool Native { get; }

    public string Name { get; }

    public byte[] OpCodes { get; }

    public int MaxSlotSize { get; }

    public Func<IObject[], bool, IObject>? Delegate { get; }

    public CompiledFunction(string name, bool natvie, byte[] opcodes, int maxSlotSize, MethodInfo methodInfo)
    {
        Name = name;
        OpCodes = opcodes;
        Native = natvie;
        MaxSlotSize = maxSlotSize;
        if (methodInfo != null)
        {
            Delegate = (args, thr) =>
            {
                return methodInfo.Invoke(null, new object[] { args, false }) as IObject;
            };
        }
    }
}