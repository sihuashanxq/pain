
namespace Pain.Compilers.CodeGen;

public class FunctionFrame
{
    public int Slot { get; internal set; }

    public int MaxSlot { get; internal set; }

    public int StackSize { get; internal set; }

    public int MaxStackSize { get; internal set; }
}