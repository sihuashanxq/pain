namespace Pain.Compilers.CodeGen;

public class FunctionEmitter
{
    internal int _offset;

    internal Scope _scope;

    internal FunctionContext _function;

    internal readonly List<OpCode> _codes;

    public FunctionEmitter(FunctionContext function)
    {
        _offset = 0;
        _codes = new List<OpCode>();
        _scope = new Scope(function.Frame, null!);
        _function = function;
    }

    public IDisposable NewScope()
    {
        _scope = new Scope(_function.Frame, _scope);

        return new Disposable(() =>
        {
            _scope.Dispose();
            _scope = _scope._parent;
        });
    }

    public Label GetLabel(string name)
    {
        return _scope.GetLabel(name)!;
    }

    public Label CreateLabel(string name)
    {
        return _scope.CreateLabel(name);
    }

    public void BindLabel(string name)
    {
        BindLabel(name, _offset);
    }

    public void BindLabel(string name, int target)
    {
        var label = GetLabel(name);
        if (label == null)
        {
            throw new Exception($"label {name} not found!");
        }

        BindLabel(label, target);
    }

    public void BindLabel(Label label)
    {
        BindLabel(label, _offset);
    }

    public void BindLabel(Label label, int target)
    {
        label.Target.Value = target;
    }

    public Variable GetVariable(string name)
    {
        return _scope.GetVariable(name)!;
    }

    public Variable GetOrCreateVariable(string name)
    {
        return _scope.GetOrCreateVariable(name);
    }

    public int Emit(OpCodeType opCodeType)
    {
        return Emit(opCodeType, null as Operand);
    }

    public int Emit(OpCodeType opCodeType, Operand? operand)
    {
        var stack = 0;
        var opCode = new OpCode(opCodeType, operand!);

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
            case OpCodeType.Ldfld:
            case OpCodeType.Brtrue:
            case OpCodeType.Brfalse:
                stack = -1;
                break;
            case OpCodeType.Stloc:
                stack = -3;
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
            case OpCodeType.Pop:
                stack = -(int)(operand!.GetValue());
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
        /*
        var id = _function.Type.Module.Pooling(v);

        return Emit(opCodeType, id);
        */
        return 0;
    }

    public int Emit(OpCodeType opCodeType, double v)
    {
        /* var id = _function.Type.Module.Pooling(v);
         return Emit(opCodeType, id);
         */
        return 0;
    }

    public void Emit(OpCode opCode)
    {
        _offset += opCode.Size;
        _codes.Add(opCode);
    }
}

internal class Scope : IDisposable
{
    public readonly int _depth;

    public readonly Scope _parent;

    public readonly FunctionFrame _frame;

    public readonly Dictionary<string, Label> _labels;

    public readonly Dictionary<string, Variable> _variables;

    public Scope(FunctionFrame frame, Scope parent)
    {
        _depth = (parent?._depth ?? 0) + 1;
        _frame = frame;
        _parent = parent!;
        _labels = new Dictionary<string, Label>();
        _variables = new Dictionary<string, Variable>();
    }

    public Label? GetLabel(string name)
    {
        if (_labels.TryGetValue(name, out var varInfo))
        {
            return varInfo;
        }

        if (_parent != null)
        {
            return _parent.GetLabel(name);
        }

        return null;
    }

    public Label CreateLabel(string name)
    {
        if (_labels.TryGetValue(name, out var label))
        {
            if (label.Target.Value == -1)
            {
                return label;
            }

            throw new Exception($"label {name} has already exists");
        }

        return _labels[name] = new Label(name, null!);
    }

    public Variable? GetVariable(string name)
    {
        if (_variables.TryGetValue(name, out var varInfo))
        {
            return varInfo;
        }

        if (_parent != null)
        {
            return _parent.GetVariable(name);
        }

        return null;
    }

    public Variable GetOrCreateVariable(string name)
    {
        if (_variables.TryGetValue(name, out var varInfo))
        {
            return varInfo;
        }

        return _variables[name] = new Variable(name, AllocateSlot(), false, null!);
    }

    public void AddStack(int n = 1)
    {
        _frame.StackSize += n;
        _frame.MaxStackSize = Math.Max(_frame.StackSize, _frame.MaxStackSize);
    }

    public void ReleaseStack(int n = 1)
    {
        _frame.StackSize -= n;
    }

    public int AllocateSlot(int n = 1)
    {
        var size = _frame.Slot;

        _frame.Slot += n;
        _frame.MaxSlot = Math.Max(_frame.MaxSlot, _frame.Slot);

        return size;
    }

    public void ReleaseSlot(int n = 1)
    {
        _frame.Slot -= n;
    }

    public void Dispose()
    {
        ReleaseSlot(_variables.Count);
    }
}