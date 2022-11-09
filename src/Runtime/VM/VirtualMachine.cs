using Pain.Compilers.CodeGen;
using Pain.Runtime.Types;
namespace Pain.Runtime.VM;
public class VirtualMachine
{
    private readonly Strings _strings;

    private readonly ClassLoader _classLoader;

    private readonly ExecutionContextStack _stack;

    public VirtualMachine(ClassLoader classLoader, Strings strings)
    {
        _stack = new ExecutionContextStack();
        _strings = strings;
        _classLoader = classLoader;
    }

    public IObject? Execute(Function function, IObject[] arguments, out bool @throw)
    {
        @throw = false;
        if (function.Func.Native)
        {
            return function.Func.Delegate(arguments, false);
        }

        using (_stack.Push(function, arguments))
        {
            var ctx = _stack.Current!;
            while (ctx.CanExecution())
            {
                var opCodeType = (OpCodeType)ctx.ReadByte();
                ctx.IP++;
                switch (opCodeType)
                {
                    case OpCodeType.Add:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.Add(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Mod:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.Mod(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Mul:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.Mul(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Neg:
                        break;
                    case OpCodeType.Sub:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.Sub(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Div:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.Div(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Shl:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.LeftShfit(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Shr:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.RightShfit(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Xor:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.Xor(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Or:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.Or(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.And:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.And(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Not:
                        {
                            var v1 = ctx.Stack.Pop();
                            var value = v1.Not(this, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Gt:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.GreaterThan(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Gte:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.GtreaterThanOrEqual(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Eq:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var value = v1.Euqal(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }

                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Neq:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            var v = v1.Euqal(this, v2, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return v;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }
                            ctx.Stack.Push(new Types.Boolean(!v.ToBoolean(this)));
                        }
                        break;
                    case OpCodeType.Pop:
                        {
                            var v = ctx.ReadInt32();
                            for (var i = 1; i <= v; i++)
                            {
                                ctx.Stack.Pop();
                            }
                            ctx.IP += 4;
                        }
                        break;
                    case OpCodeType.Ldloc:
                        {
                            var v = ctx.Varaibles[ctx.ReadInt32()];
                            ctx.Stack.Push(v);
                            ctx.IP += 4;
                        }
                        break;
                    case OpCodeType.Ldarg:
                        {
                            var v = ctx.ReadInt32();
                            ctx.Stack.Push(ctx.Arguments[v]);
                            ctx.IP += 4;
                        }
                        break;
                    case OpCodeType.Ldnull:
                        {
                            ctx.Stack.Push(Null.Value);
                        }
                        break;
                    case OpCodeType.Ldfld:
                        {
                            var name = ctx.Stack.Pop();
                            var obj = ctx.Stack.Pop();
                            var value = obj.GetField(this, name, out @throw);
                            if (@throw)
                            {
                                if (ctx.Exception == null)
                                {
                                    return value;
                                }
                                else
                                {
                                    ctx.IP = ctx.Exception.Catch;
                                    break;
                                }
                            }

                            ctx.Stack.Push(value);
                        }
                        break;
                    case OpCodeType.Ldstr:
                        {
                            var token = ctx.ReadInt32();
                            var str = _strings.GetString(token);
                            ctx.Stack.Push(new Types.String(str));
                            ctx.IP += 4;
                        }
                        break;
                    case OpCodeType.Ldnum:
                        {
                            ctx.Stack.Push(new Number(ctx.ReadDouble()));
                            ctx.IP += 8;
                        }
                        break;
                    case OpCodeType.Stloc:
                        {
                            var v = ctx.Stack.Pop();
                            var idx = ctx.ReadInt32();
                            ctx.Varaibles[idx] = v;
                            ctx.IP += 4;
                        }
                        break;
                    case OpCodeType.Stfld:
                        {
                            var value = ctx.Stack.Pop();
                            var name = ctx.Stack.Pop();
                            var obj = ctx.Stack.Pop();
                            obj.SetField(this, name, value, out @throw);
                        }
                        break;
                    case OpCodeType.Ldtoken:
                        {
                            var name = ctx.Stack.Pop().ToString();
                            var module = ctx.Stack.Pop().ToString();
                            var token = new ModuleToken(module!, name!);
                            var @class = _classLoader.Load(token);
                            ctx.Stack.Push(@class);
                        }
                        break;
                    case OpCodeType.New:
                        {
                            var @class = ctx.Stack.Pop() as Runtime.Types.Type;
                            ctx.Stack.Push(@class!.CreateInstance());
                        }
                        break;
                    case OpCodeType.Ret:
                        {
                            return ctx.Stack.Pop();
                        }
                    case OpCodeType.Brfalse:
                        {
                            var v = ctx.Stack.Pop();
                            if (!v.ToBoolean(this))
                            {
                                ctx.IP = ctx.ReadInt32();
                            }
                            else
                            {
                                ctx.IP += 4;
                            }
                        }
                        break;
                    case OpCodeType.Brtrue:
                        {
                            var v = ctx.Stack.Pop();
                            if (v.ToBoolean(this))
                            {
                                ctx.IP = ctx.ReadInt32();
                                break;
                            }
                            ctx.IP += 4;
                            break;
                        }
                    case OpCodeType.Br:
                        ctx.IP = ctx.ReadInt32();
                        break;
                    case OpCodeType.Call:
                        {
                            var n = ctx.ReadInt32();
                            var args = new IObject[n];
                            for (var i = n - 1; i >= 0; i--)
                            {
                                args[i] = ctx.Stack.Pop();
                            }

                            var func = ctx.Stack.Pop();
                            var value = func.Call(this, args, out @throw);
                            if (@throw)
                            {

                            }
                            ctx.Stack.Push(value!);
                            ctx.IP += 4;
                        }
                        break;
                    case OpCodeType.Dup:
                        {
                            ctx.Stack.Push(ctx.Stack.Peek());
                        }
                        break;
                    case OpCodeType.Swap1_2:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1);
                            ctx.Stack.Push(v2);
                        }
                        break;
                    case OpCodeType.LdLabel:
                        {
                            var v1 = ctx.ReadInt32();
                            ctx.Stack.Push(new Number(v1));
                            ctx.IP += 4;
                        }
                        break;
                    case OpCodeType.Try:
                        var f = (int)(ctx.Stack.Pop() as Number).Value;
                        var c = (int)(ctx.Stack.Pop() as Number).Value;
                        ctx.Exceptions.Push(new ExceptionMachineState()
                        {
                            State = State.Try,
                            Catch = c,
                            Finally = f
                        });
                        break;
                    case OpCodeType.EndTry:
                        ctx.Exception!.State = State.EndTry;
                        break;
                    case OpCodeType.Catch:
                        @throw = false;
                        ctx.Exception!.Value = Null.Value;
                        ctx.Exception!.Throw = false;
                        ctx.Exception!.State = State.Catch;
                        break;
                    case OpCodeType.EndCatch:
                        ctx.Exception!.State = State.EndCatch;
                        break;
                    case OpCodeType.Throw:
                        @throw = true;
                        if (ctx.Exception == null)
                        {
                            return ctx.Stack.Pop();
                        }

                        if (ctx.Exception.State == State.Try)
                        {
                            @throw = false;
                            ctx.Exception!.Value = ctx.Stack.Pop();
                            ctx.Exception!.Throw = true;
                            ctx.IP = ctx.Exception.Catch;
                        }
                        else if (ctx.Exception.State == State.Catch)
                        {
                            @throw = true;
                            ctx.Exception!.Value = ctx.Stack.Pop();
                            ctx.Exception!.Throw = true;
                            ctx.IP = ctx.Exception.Finally;
                        }
                        else
                        {
                            return ctx.Stack.Pop();
                        }
                        break;
                    case OpCodeType.Finally:
                        ctx.Exception!.State = State.Finally;
                        break;
                    case OpCodeType.EndFinally:
                        ctx.Exception!.State = State.EndFinally;
                        var ex = ctx.Exceptions.Pop();
                        if (ex.Throw)
                        {
                            @throw = true;
                            return ex.Value;
                        }
                        break;
                }
            }

            return Null.Value;
        }
    }

    public IObject? Execute(ModuleToken token, string function, IObject[] arguments, out bool @throw)
    {
        var @class = _classLoader.Load(token);
        var func = @class.GetFunction(this, Null.Value, new Types.String(function));

        return Execute(func!, arguments, out @throw);
    }

    public ClassLoader GetClassLoader()
    {
        return _classLoader;
    }
}

internal class ExecutionContext
{
    public int IP { get; internal set; }

    public System.Collections.Generic.Stack<IObject> Stack { get; }

    public Function Function { get; }

    public IObject[] Arguments { get; }

    public IObject[] Varaibles { get; }

    public ExceptionMachineState? Exception
    {
        get
        {
            if (Exceptions.Count == 0)
            {
                return null;
            }

            return Exceptions.Peek();
        }
    }
    public Stack<ExceptionMachineState> Exceptions { get; }

    public ExecutionContext(Function function, IObject[] arguments)
    {
        IP = 0;
        Stack = new System.Collections.Generic.Stack<IObject>();
        Function = function;
        Arguments = arguments;
        Varaibles = new IObject[function.Func.MaxSlotSize];
        Exceptions = new Stack<ExceptionMachineState>();
    }

    public byte ReadByte()
    {
        return Function.Func.OpCodes[IP];
    }

    public Int32 ReadInt32()
    {
        return BitConverter.ToInt32(Function.Func.OpCodes, IP);
    }

    public Int64 ReadInt64()
    {
        return BitConverter.ToInt64(Function.Func.OpCodes, IP);
    }

    public double ReadDouble()
    {
        return BitConverter.ToDouble(Function.Func.OpCodes, IP);
    }

    public bool CanExecution()
    {
        return IP < Function.Func.OpCodes.Length;
    }
}

internal class ExecutionContextStack
{
    private ExecutionContext? _current;

    private Stack<ExecutionContext> _stack;

    internal ExecutionContext? Current => _current;

    internal ExecutionContextStack()
    {
        _stack = new Stack<ExecutionContext>();
    }

    internal void Pop()
    {
        _current = _stack.Pop();
    }

    internal IDisposable Push(Function function, IObject[] arguments)
    {
        var ctx = new ExecutionContext(function, arguments);
        _current = ctx;
        _stack.Push(ctx);
        return new Disposable(Pop);
    }
}

internal class ExceptionMachineState
{
    public bool Throw;

    public IObject Value;

    public int Catch;

    public int Finally;

    public State State = State.None;
}

public enum State
{
    None,
    Try,

    EndTry,

    Catch,

    EndCatch,

    Finally,

    EndFinally
}
