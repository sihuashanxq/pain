
namespace Pain.Compilers.CodeGen;

public class Frame
{
    public int VarSlot { get; internal set; }

    public int MaxVarSlot { get; internal set; }

    public int StackSize { get; internal set; }

    public int MaxStackSize { get; internal set; }
}