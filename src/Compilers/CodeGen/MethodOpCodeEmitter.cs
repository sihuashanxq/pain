namespace Pain.Compilers.CodeGen;

public class MethodOpCodeEmitter
{
    internal int _scope;

    internal int _offset;
    
    internal MethodContext _method;

    internal readonly List<OpCode> _codes;

    public MethodOpCodeEmitter(MethodContext method)
    {
        _scope = 0;
        _offset = 0;
        _codes = new List<OpCode>();
        _method = method;
    }

    public int Emit(OpCodeType opCodeType, Operand operand)
    {
        var stack = 0;
        var opCode = new OpCode(opCodeType, operand);

        switch (opCodeType)
        {
            case OpCodeType.Ret:
            case OpCodeType.Add: //+
            case OpCodeType.Sub: //-
            case OpCodeType.Mul: //*
            case OpCodeType.Div: ///
            case OpCodeType.Mod: //%
            case OpCodeType.Shl: //<<
            case OpCodeType.Shr: //>>
            case OpCodeType.Xor: //^
            case OpCodeType.Eq: //==
            case OpCodeType.Gt: //>
            case OpCodeType.Gte: //>=
            case OpCodeType.Or: //|
            case OpCodeType.And: //&
            case OpCodeType.Pop:
            case OpCodeType.Ldfld:
            case OpCodeType.Stloc:
            case OpCodeType.Brtrue:
            case OpCodeType.Brfalse:
                stack = -1;
                break;
            case OpCodeType.Call:
            case OpCodeType.New:
                stack = -(operand as Operand<int>)!.Value;
                break;
            case OpCodeType.Br:
            case OpCodeType.Not:
            case OpCodeType.Nop:
            case OpCodeType.Swap1_2:
                stack = 0;
                break;
            case OpCodeType.Dup:
            case OpCodeType.Push:
            case OpCodeType.Ldloc:
            case OpCodeType.Ldnull:
            case OpCodeType.Ldundf:
            case OpCodeType.Ldstr:
            case OpCodeType.Ldnum:
                stack = 1;
                break;
            case OpCodeType.Pop2:
                stack = -2;
                break;
            case OpCodeType.Pop3:
            case OpCodeType.Stfld:
                stack = -3;
                break;
            case OpCodeType.Pop4:
                stack = -4;
                break;
            case OpCodeType.Popn:
                switch ((operand as Operand<int>)!.Value)
                {
                    case 0:
                        return 0;
                    case 1:
                        return Emit(OpCodeType.Pop);
                    case 2:
                        return Emit(OpCodeType.Pop2);
                    case 3:
                        return Emit(OpCodeType.Pop3);
                    case 4:
                        return Emit(OpCodeType.Popn);
                    default:
                        stack = -stack;
                        break;
                }
                break;
        }

        Emit(opCode);

        return stack;
    }

    public int Emit(OpCodeType opCodeType, int v)
    {
        return Emit(opCodeType, new Operand<int>(v, sizeof(int)));
    }

    public int Emit(OpCodeType opCodeType, string v)
    {
        var id = _method.Module.Pooling(v);

        return Emit(opCodeType, id);
    }

    public int Emit(OpCodeType opCodeType, double v)
    {
        var id = _method.Module.Pooling(v);

        return Emit(opCodeType, id);
    }

    public void Emit(OpCode opCode)
    {
        _offset += opCode.Size;
        _codes.Add(opCode);
    }
}