using Pain.Runtime;
using Pain.Compilers.CodeGen;
namespace Pain.VM;
public class VM
{
    private readonly ClassLoader _classLoader;

    private readonly ExecutionContextStack _stack;

    public VM(ClassLoader classLoader)
    {
        _stack = new ExecutionContextStack();
        _classLoader = classLoader;
    }

    public Runtime.BaseObject Execute(FunctionObject function, Runtime.BaseObject[] arguments)
    {
        using (_stack.Push(function, arguments))
        {
            var ctx = _stack.Current!;
            while (ctx.CanExecution())
            {
                switch ((OpCodeType)ctx.ReadByte())
                {
                    case OpCodeType.Add:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorAdd(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Mod:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorMod(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Mul:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorMul(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Neg:
                        break;
                    case OpCodeType.Sub:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorSub(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Div:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorDiv(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Shl:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorLeftShfit(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Shr:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorRightShfit(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Xor:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorXor(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Or:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorOr(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.And:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorAnd(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Not:
                        {
                            var v1 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorNot());
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Gt:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.__GreaterThan__(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Gte:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.OperatorGreatherThanOrEqual(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Eq:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1.__Euqal__(v2));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Pop:
                        {
                            var v = ctx.ReadInt32();
                            for (var i = 0; i < v; i++)
                            {
                                ctx.Stack.Pop();
                            }
                            ctx.IP += 5;
                        }
                        break;
                    case OpCodeType.Ldloc:
                        {
                            var v = ctx.Varaibles[ctx.ReadInt32()];
                            ctx.Stack.Push(v);
                            ctx.IP += 5;
                        }
                        break;
                    case OpCodeType.Ldarg:
                        {
                            var v = ctx.ReadInt32();
                            ctx.Stack.Push(ctx.Arguments[v]);
                            ctx.IP += 5;
                        }
                        break;
                    case OpCodeType.Ldnull:
                        {
                            ctx.Stack.Push(Runtime.BaseObject.Null);
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Ldfld:
                        {
                            var obj = ctx.Stack.Pop();
                            var name = ctx.Stack.Pop();
                            ctx.Stack.Push(obj.GetField(name));
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Ldstr:
                        /*
                            {
                                var id = codes.ReadInt32();
                                var v2 = _main.Module.Strings.GetPooling(id);
                                _stack._stack.Push(v2);
                                _ip += 5;
                            }
                            */
                        break;
                    case OpCodeType.Ldnum:
                        /*
                            {
                                var id = codes.ReadInt32();
                                var v2 = _main.Module.Numbers.GetPooling(id);
                                _stack._stack.Push(v2);
                                _ip += 5;
                            }
                            break;
                            */
                        break;
                    case OpCodeType.Stloc:
                        {
                            var v = ctx.Stack.Pop();
                            var idx = ctx.ReadInt32();
                            ctx.Varaibles[idx] = v;
                            ctx.IP += 5;
                        }
                        break;
                    case OpCodeType.Stfld:
                        {
                            var obj = ctx.Stack.Pop();
                            var name = ctx.Stack.Pop();
                            var value = ctx.Stack.Pop();
                            obj.SetField(name, value);
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Ldtoken:
                        {
                            var token = ctx.Stack.Pop().ToString()!;
                            var metadata = _classLoader.Load(token);
                            ctx.Stack.Push(metadata);
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.New:
                        {
                            var metadata = ctx.Stack.Pop() as ClassObject;
                            ctx.Stack.Push(metadata!.CreateInstance());
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Ret:
                        {
                            return ctx.Stack.Pop();
                        }
                    case OpCodeType.Brfalse:
                        {
                            var v = ctx.Stack.Pop();
                            if (!v.IsTrue())
                            {
                                ctx.IP = ctx.ReadInt32();
                            }
                            else
                            {
                                ctx.IP += 5;
                            }
                        }
                        break;
                    case OpCodeType.Brtrue:
                        {
                            var v = ctx.Stack.Pop();
                            if (v.IsTrue())
                            {
                                ctx.IP = ctx.ReadInt32();
                            }
                            else
                            {
                                ctx.IP += 5;
                            }
                        }
                        break;
                    case OpCodeType.Br:
                        {
                            ctx.IP = ctx.ReadInt32();
                        }
                        break;
                    case OpCodeType.Call:
                        {
                            var n = ctx.ReadInt32();
                            var args = new Runtime.BaseObject[n];
                            for (var i = n - 1; i >= 0; i--)
                            {
                                arguments[i] = ctx.Stack.Pop();
                            }

                            var func = ctx.Stack.Pop() as FunctionObject;
                            var value = Execute(func!, new[] { func!.Target }.Concat(args).ToArray());
                            ctx.Stack.Push(value);
                            ctx.IP += 5;
                        }
                        break;
                    case OpCodeType.Dup:
                        {
                            ctx.Stack.Push(ctx.Stack.Peek());
                            ctx.IP += 1;
                        }
                        break;
                    case OpCodeType.Swap1_2:
                        {
                            var v1 = ctx.Stack.Pop();
                            var v2 = ctx.Stack.Pop();
                            ctx.Stack.Push(v1);
                            ctx.Stack.Push(v2);
                            ctx.IP += 1;
                        }
                        break;
                }
            }

            return Runtime.BaseObject.Null;
        }
    }
}

internal class ExecutionContext
{
    public int IP { get; internal set; }

    public Stack<Runtime.BaseObject> Stack { get; }

    public FunctionObject Function { get; }

    public Runtime.BaseObject[] Arguments { get; }

    public Runtime.BaseObject[] Varaibles { get; }

    public ExecutionContext(FunctionObject function, Runtime.BaseObject[] arguments)
    {
        IP = 0;
        Stack = new Stack<Runtime.BaseObject>();
        Function = function;
        Arguments = arguments;
        Varaibles = new Runtime.BaseObject[function.Function.MaxStackSize];
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

    internal IDisposable Push(FunctionObject function, Runtime.BaseObject[] arguments)
    {
        var ctx = new ExecutionContext(function, arguments);
        _current = ctx;
        _stack.Push(ctx);
        return new Disposable(Pop);
    }
}