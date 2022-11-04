namespace Pain.Compilers.CodeGen;

public class OpCode
{
    public int Size { get; }

    public OpCodeType Type { get; }

    public Operand? Operand { get; }

    public OpCode(OpCodeType type, Operand operand)
    {
        Type = type;
        Size = sizeof(OpCodeType) + (operand?.Size ?? 0);
        Operand = operand;
    }

    public void WriteTo(Stream stream)
    {
        stream.WriteByte((byte)Type);
        Operand?.WriteTo(stream);
    }

    public override string ToString()
    {
        if (Type == OpCodeType.Ldstr)
        {
            return $"{Type}  {(ModuleCompiler.Strings.GetString((int)Operand.GetValue()))} \t";
        }
        else if (Type == OpCodeType.Ldnum)
        {
            return $"{Type} {Operand.GetValue()}";
        }
        else if (Type == OpCodeType.Ldfld)
        {
             return $"{Type}  {(ModuleCompiler.Strings.GetString((int)Operand.GetValue()))} \t";
        }

        return $"{Type}  {(Operand)} \t";
    }
}