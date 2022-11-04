using Pain.Compilers.CodeGen;
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

    public IObject? Execute(RuntimeFunction function, IObject[] arguments)
    {
        if (function.Function.Native)
        {
            return function.Function.Delegate(arguments);
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
                            ctx.Stack.Push(v1.Add(this, v2));
                        }
                        break;
                    case OpCodeType.Mod:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.Mod(this, v2));
                        }
                        break;
                    case OpCodeType.Mul:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.Mul(this, v2));
                        }
                        break;
                    case OpCodeType.Neg:
                        break;
                    case OpCodeType.Sub:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.Sub(this, v2));
                        }
                        break;
                    case OpCodeType.Div:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.Div(this, v2));
                        }
                        break;
                    case OpCodeType.Shl:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.LeftShfit(this, v2));
                        }
                        break;
                    case OpCodeType.Shr:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.RightShfit(this, v2));
                        }
                        break;
                    case OpCodeType.Xor:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.Xor(this, v2));
                        }
                        break;
                    case OpCodeType.Or:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.Or(this, v2));
                        }
                        break;
                    case OpCodeType.And:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.And(this, v2));
                        }
                        break;
                    case OpCodeType.Not:
                        {
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.Not(this));
                        }
                        break;
                    case OpCodeType.Gt:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.GreaterThan(this, v2));
                        }
                        break;
                    case OpCodeType.Gte:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.GtreaterThanOrEqual(this, v2));
                        }
                        break;
                    case OpCodeType.Eq:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.Euqal(this, v2));
                        }
                        break;
                        case OpCodeType.Neq:
                        {
                            var v2 = ctx.Stack.Pop();
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(new RuntimeBoolean(!v1.Euqal(this, v2).ToBoolean(this)));
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
                            ctx.Stack.Push(RuntimeNull.Null);
                        }
                        break;
                    case OpCodeType.Ldfld:
                        {
                            var name = ctx.Stack.Pop();
                            var obj = ctx.Stack.Pop();
                            ctx.Stack.Push(obj.GetField(this, name));
                        }
                        break;
                    case OpCodeType.Ldstr:
                        {
                            var token = ctx.ReadInt32();
                            var str = _strings.GetString(token);
                            ctx.Stack.Push(new RuntimeString(str));
                            ctx.IP += 4;
                        }
                        break;
                    case OpCodeType.Ldnum:
                        {
                            ctx.Stack.Push(new RuntimeNumber(ctx.ReadDouble()));
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
                            obj.SetField(this, name, value);
                        }
                        break;
                    case OpCodeType.Ldtoken:
                        {
                            var token = ctx.Stack.Pop().ToString()!;
                            var @class = _classLoader.Load(token);
                            ctx.Stack.Push(@class);
                        }
                        break;
                    case OpCodeType.New:
                        {
                            var @class = ctx.Stack.Pop() as RuntimeClass;
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
                            var value = func.Call(this, args);
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
                }
            }

            return RuntimeNull.Null;
        }
    }

    public IObject? Execute(string module, string @class, string function, IObject[] arguments)
    {
        var token = $"{module}.{@class}";
        var runtimeClass = _classLoader.Load(token);
        var func = runtimeClass.GetFunction(this, RuntimeNull.Null, new RuntimeString(function));

        return Execute(func!, arguments);
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

    public RuntimeFunction Function { get; }

    public IObject[] Arguments { get; }

    public IObject[] Varaibles { get; }

    public ExecutionContext(RuntimeFunction function, IObject[] arguments)
    {
        IP = 0;
        Stack = new System.Collections.Generic.Stack<IObject>();
        Function = function;
        Arguments = arguments;
        Varaibles = new IObject[function.Function.MaxSlotSize];
    }

    public byte ReadByte()
    {
        return Function.Function.OpCodes[IP];
    }

    public Int32 ReadInt32()
    {
        return BitConverter.ToInt32(Function.Function.OpCodes, IP);
    }

    public Int64 ReadInt64()
    {
        return BitConverter.ToInt64(Function.Function.OpCodes, IP);
    }

    public double ReadDouble()
    {
        return BitConverter.ToDouble(Function.Function.OpCodes, IP);
    }

    public bool CanExecution()
    {
        return IP < Function.Function.OpCodes.Length;
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

    internal IDisposable Push(RuntimeFunction function, IObject[] arguments)
    {
        var ctx = new ExecutionContext(function, arguments);
        _current = ctx;
        _stack.Push(ctx);
        return new Disposable(Pop);
    }
}
