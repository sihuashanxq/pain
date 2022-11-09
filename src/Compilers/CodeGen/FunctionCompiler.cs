
using Pain.Compilers.Expressions;
using Pain.Runtime;
namespace Pain.Compilers.CodeGen;

public class FunctionCompiler : Expressions.SyntaxVisitor<int>
{
    private readonly FunctionContext _function;

    private readonly FunctionEmitter _emitter;

    public FunctionCompiler(FunctionContext function, Strings strings)
    {
        _emitter = new FunctionEmitter(function, strings);
        _function = function;
    }

    public CompiledFunction Compile()
    {
        if (_function.Native)
        {
            return new CompiledFunction(_function.Name, _function.Native, null!, 0, null!);
        }

        Visit(_function.Expression);
        return new CompiledFunction(_function.Name, false, _emitter.GetBuffer(), _function.Frame.MaxSlot, null!);
    }

    public static CompiledFunction CompileFunction(FunctionContext function, Strings strings)
    {
        return new FunctionCompiler(function, strings).Compile();
    }

    protected internal override int VisitBinary(BinaryExpression expr)
    {
        var stack = 0;

        if (expr.Type == SyntaxType.Assign)
        {
            return VisitAssign(expr);
        }

        if (expr.Type == SyntaxType.Or)
        {
            using (_emitter.Scope())
            {
                var next = _emitter.CreateLabel(Label.Next);

                stack += expr.Left.Accept(this);
                stack += _emitter.Emit(OpCodeType.Dup);
                stack += _emitter.Emit(OpCodeType.Brtrue, next.Target);
                stack += _emitter.Emit(OpCodeType.Pop, 1);
                stack += expr.Right.Accept(this);

                _emitter.BindLabel(next);
                return stack;
            }
        }

        if (expr.Type == SyntaxType.And)
        {
            using (_emitter.Scope())
            {
                var next = _emitter.CreateLabel(Label.Next);

                stack += expr.Left.Accept(this);
                stack += _emitter.Emit(OpCodeType.Dup);
                stack += _emitter.Emit(OpCodeType.Brfalse, next.Target);
                stack += _emitter.Emit(OpCodeType.Pop, 1);
                stack += expr.Right.Accept(this);

                _emitter.BindLabel(next);
                return stack;
            }
        }

        if (expr.Type == SyntaxType.LessThan ||
            expr.Type == SyntaxType.LessThanOrEqual)
        {
            stack += expr.Right.Accept(this);
            stack += expr.Left.Accept(this);
        }
        else
        {
            stack += expr.Left.Accept(this);
            stack += expr.Right.Accept(this);
        }

        switch (expr.Type)
        {
            case SyntaxType.Add:
                stack += _emitter.Emit(OpCodeType.Add);
                break;
            case SyntaxType.Subtract:
                stack += _emitter.Emit(OpCodeType.Sub);
                break;
            case SyntaxType.Multiply:
                stack += _emitter.Emit(OpCodeType.Mul);
                break;
            case SyntaxType.Divide:
                stack += _emitter.Emit(OpCodeType.Div);
                break;
            case SyntaxType.Modulo:
                stack += _emitter.Emit(OpCodeType.Mod);
                break;
            case SyntaxType.BitLeftShift:
                stack += _emitter.Emit(OpCodeType.Shl);
                break;
            case SyntaxType.BitRightShift:
                stack += _emitter.Emit(OpCodeType.Shr);
                break;
            case SyntaxType.BitXor:
                stack += _emitter.Emit(OpCodeType.Xor);
                break;
            case SyntaxType.BitOr:
                stack += _emitter.Emit(OpCodeType.Or);
                break;
            case SyntaxType.And:
                stack += _emitter.Emit(OpCodeType.And);
                break;
            case SyntaxType.EqualTo:
                stack += _emitter.Emit(OpCodeType.Eq);
                break;
            case SyntaxType.NotEqualTo:
                stack += _emitter.Emit(OpCodeType.Neq);
                break;
            case SyntaxType.LessThan:
            case SyntaxType.GreaterThan:
                stack += _emitter.Emit(OpCodeType.Gt);
                break;
            case SyntaxType.LessThanOrEqual:
            case SyntaxType.GreaterThanOrEqual:
                stack += _emitter.Emit(OpCodeType.Gte);
                break;
            default:
                throw new Exception($"not supported {expr.Type}");
        }

        return stack;
    }

    protected internal virtual int VisitAssign(BinaryExpression expr)
    {
        var stack = 0;
        using (_emitter.Scope())
        {
            switch (expr.Left.Type)
            {
                case SyntaxType.Name:
                    var name = expr.Left as NameExpression;
                    var variable = _emitter.GetVariable(name.Name);
                    if (variable == null)
                    {
                        throw new Exception($"{name.Name} was not found");
                    }
                    stack += expr.Right.Accept(this);
                    stack += _emitter.Emit(OpCodeType.Stloc, variable);
                    return stack.AreEqual(0);
                case SyntaxType.Member:
                    var member = expr.Left as MemberExpression;
                    stack += member!.Object.Accept(this);
                    stack += member.Member.Accept(this);
                    stack += expr.Right.Accept(this);
                    stack += _emitter.Emit(OpCodeType.Stfld);
                    return stack.AreEqual(0);
                default:
                    throw new Exception();
            }
        }
    }

    protected internal override int VisitBlock(BlockExpression blockExpression)
    {
        using (_emitter.Scope())
        {
            var stack = 0;

            foreach (var item in blockExpression.Statements)
            {
                stack += item.Accept(this);
                stack += _emitter.Emit(OpCodeType.Pop, stack);
            }

            return stack.AreEqual(0);
        }
    }

    protected internal override int VisitCall(CallExpression expr)
    {
        var stack = 0;
        stack += expr.Function.Accept(this);
        stack += expr.Arguments.Sum(argument => argument.Accept(this));
        stack += _emitter.Emit(OpCodeType.Call, expr.Arguments.Length);
        return stack.AreEqual(1);
    }

    protected internal override int VisitConstant(ConstantExpression constantExpression)
    {
        var stack = 0;

        switch (constantExpression.Type)
        {
            case SyntaxType.ConstString:
                stack += _emitter.Emit(OpCodeType.Ldstr, constantExpression.Value.ToString()!);
                break;
            case SyntaxType.ConstNumber:
                stack += _emitter.Emit(OpCodeType.Ldnum, Convert.ToDouble(constantExpression.Value));
                break;
            case SyntaxType.ConstBoolean:
                stack += _emitter.Emit(OpCodeType.Ldnum, 0d);
                stack += _emitter.Emit(OpCodeType.Ldnum, 0d);
                stack += _emitter.Emit((constantExpression.Value.ToString() == "true") ? OpCodeType.Eq : OpCodeType.Gt);
                break;
            case SyntaxType.ConstNull:
                stack += _emitter.Emit(OpCodeType.Ldnull);
                break;
            default:
                throw new Exception("unknown constant!");
        }

        return stack.AreEqual(1);
    }

    protected internal override int VisitBreak(BreakExpression breakExpression)
    {
        var label = _emitter.GetLabel(Label.Break);
        if (label == null)
        {
            throw new Exception("break");
        }

        return _emitter.Emit(OpCodeType.Br, label.Target);
    }

    protected internal override int VisitContinue(ContinueExpression continueExpression)
    {
        var label = _emitter.GetLabel(Label.Continue);
        if (label == null)
        {
            throw new Exception("continue");
        }

        return _emitter.Emit(OpCodeType.Br, label.Target);
    }

    protected internal override int VisitEmpty(EmptyExpression emptyExpression)
    {
        return 0;
    }

    protected internal override int VisitFor(ForExpression forExpression)
    {
        using (_emitter.Scope())
        {
            var end = _emitter.CreateLabel(Label.Break);
            var next = _emitter.CreateLabel(Label.Continue);
            var begin = _emitter.CreateLabel(Label.Begin);
            var stack = forExpression.Initializers.Sum(item => item.Accept(this)).AreEqual(0);
            _emitter.BindLabel(begin);

            if (forExpression.Test != null)
            {
                stack += forExpression.Test.Accept(this);
                stack += _emitter.Emit(OpCodeType.Brfalse, end.Target);
            }

            stack += forExpression.Body.Accept(this);
            _emitter.BindLabel(next);

            stack += forExpression.Iterators.Sum(item => item.Accept(this)).AreEqual(0);
            stack += _emitter.Emit(OpCodeType.Br, begin.Target);
            _emitter.BindLabel(end);

            return stack.AreEqual(0);
        }
    }

    protected internal override int VisitFunction(FunctionExpression functionExpression)
    {
        using (_emitter.Scope())
        {
            var stack = functionExpression.Body.Accept(this);
            stack += _emitter.Emit(OpCodeType.Pop, stack);
            stack += _emitter.Emit(OpCodeType.Ldnull);
            stack += _emitter.Emit(OpCodeType.Ret);
            return stack.AreEqual(0);
        }
    }

    protected internal override int VisitIf(IfExpression ifExpression)
    {
        using (_emitter.Scope())
        {
            var stack = 0;
            var ifEnd = _emitter.CreateLabel(Label.End);
            var ifTrue = _emitter.CreateLabel(Label.True);
            var ifFalse = _emitter.CreateLabel(Label.False);

            if (ifExpression.Test != null)
            {
                stack += ifExpression.Test.Accept(this).AreEqual(1);
            }
            else
            {
                stack += _emitter.Emit(OpCodeType.Ldnum, 0d);
            }

            stack += _emitter.Emit(OpCodeType.Brtrue, ifTrue.Target);
            stack += _emitter.Emit(OpCodeType.Br, ifFalse.Target);

            using (_emitter.Scope())
            {
                _emitter.BindLabel(ifTrue);

                stack += ifExpression.IfTrue.Accept(this);
                stack += _emitter.Emit(OpCodeType.Pop, stack);
                stack += _emitter.Emit(OpCodeType.Br, ifEnd.Target);
            }

            if (ifExpression.IfFalse != null)
            {
                using (_emitter.Scope())
                {
                    _emitter.BindLabel(ifFalse);

                    stack += ifExpression.IfFalse.Accept(this);
                    stack += _emitter.Emit(OpCodeType.Pop, stack);
                    stack += _emitter.Emit(OpCodeType.Br, ifEnd.Target);
                }
            }
            else
            {
                _emitter.BindLabel(ifFalse);
            }

            _emitter.BindLabel(ifEnd);
            return stack.AreEqual(0);
        }
    }

    protected internal override int VisitMember(MemberExpression expr)
    {
        var stack = expr.Object.Accept(this).AreEqual(1);
        stack += expr.Member.Accept(this).AreEqual(1);
        stack += _emitter.Emit(OpCodeType.Ldfld);
        return stack.AreEqual(1);
    }

    protected internal override int VisitName(NameExpression expr)
    {
        for (var i = 0; i < _function.Expression.Parameters.Length; i++)
        {
            var item = _function.Expression.Parameters[i];
            if (item.Name == expr.Name)
            {
                return _emitter.Emit(OpCodeType.Ldarg, i + 1);
            }
        }

        var variable = _emitter.GetVariable(expr.Name);
        if (variable != null)
        {
            return _emitter.Emit(OpCodeType.Ldloc, variable.Slot).AreEqual(1);
        }

        if (_function.Class.Module.Classes.TryGetValue(expr.Name, out var @class))
        {
            var stack = 0;
            stack += _emitter.Emit(OpCodeType.Ldstr, @class.Token.Module);
            stack += _emitter.Emit(OpCodeType.Ldstr, @class.Token.Name);
            stack += _emitter.Emit(OpCodeType.Ldtoken);
            return stack.AreEqual(1);
        }

        if (_function.Class.Module.Imports.TryGetValue(expr.Name, out var importedClass))
        {
            var stack = 0;
            stack += _emitter.Emit(OpCodeType.Ldstr, importedClass.Module);
            stack += _emitter.Emit(OpCodeType.Ldstr, importedClass.Name);
            stack += _emitter.Emit(OpCodeType.Ldtoken);
            return stack.AreEqual(1);
        }

        throw new Exception($"{expr.Name} was not found");
    }

    protected internal override int VisitNew(NewExpression expr)
    {
        var stack = 0;
        stack += expr.Class.Accept(this).AreEqual(1);
        stack += _emitter.Emit(OpCodeType.New);
        stack += _emitter.Emit(OpCodeType.Dup);
        stack += _emitter.Emit(OpCodeType.Ldstr, "ctor");
        stack += _emitter.Emit(OpCodeType.Ldfld);
        stack += expr.Arguments.Sum(argument => argument.Accept(this));
        stack += _emitter.Emit(OpCodeType.Call, expr.Arguments.Length);
        stack += _emitter.Emit(OpCodeType.Pop, 1);
        return stack.AreEqual(1);
    }

    protected internal override int VisitParameter(ParameterExpression expr)
    {
        return 0;
    }

    protected internal override int VisitReturn(ReturnExpression expr)
    {
        var stack = 0;

        if (expr.Value != null)
        {
            stack = expr.Value.Accept(this).AreEqual(1);
            stack += _emitter.Emit(OpCodeType.Ret);
        }
        else
        {
            stack += _emitter.Emit(OpCodeType.Ldnull);
            stack += _emitter.Emit(OpCodeType.Ret);
        }

        return stack.AreEqual(0);
    }

    protected internal override int VisitSuper(SuperExpression expr)
    {
        throw new Exception("Super");
    }

    protected internal override int VisitThis(ThisExpression expr)
    {
        return _emitter.Emit(OpCodeType.Ldarg, 0).AreEqual(1);
    }

    protected internal override int VisitUnary(UnaryExpression expr)
    {
        var stack = expr.Expression.Accept(this);
        var tokenType = expr.Type;

        switch (tokenType)
        {
            case SyntaxType.Not:
                stack += _emitter.Emit(OpCodeType.Ldnum, 0d);
                stack += _emitter.Emit(OpCodeType.Eq);
                break;
            case SyntaxType.BitNot:
                stack += _emitter.Emit(OpCodeType.Not);
                break;
            case SyntaxType.Subtract:
                stack += _emitter.Emit(OpCodeType.Neg);
                break;
            case SyntaxType.Add:
                break;
            default:
                break;
        }

        return stack.AreEqual(1);
    }

    protected internal override int VisitVariable(VariableExpression expr)
    {
        var stack = 0;

        foreach (var item in expr.Varaibles)
        {
            var variable = _emitter.CreateVariable(item.Name);
            if (item.Value != null)
            {
                stack += item.Value.Accept(this).AreEqual(1);
                stack += _emitter.Emit(OpCodeType.Stloc, variable);
            }
        }

        return stack.AreEqual(0);
    }

    protected internal override int VisitArrayInit(ArrayInitExpression expr)
    {
        var stack = 0;
        stack += _emitter.Emit(OpCodeType.Ldstr, "Runtime.Array");
        stack += _emitter.Emit(OpCodeType.Ldtoken);
        stack += _emitter.Emit(OpCodeType.New);
        stack += _emitter.Emit(OpCodeType.Dup);
        stack += _emitter.Emit(OpCodeType.Ldstr, "ctor");
        stack += _emitter.Emit(OpCodeType.Ldfld);
        stack += _emitter.Emit(OpCodeType.Call, 0);
        stack += _emitter.Emit(OpCodeType.Pop, 1);

        for (var i = 0; i < expr.Items.Length; i++)
        {
            stack += _emitter.Emit(OpCodeType.Dup);
            stack += _emitter.Emit(OpCodeType.Ldnum, i);
            stack += expr.Items[i].Accept(this).AreEqual(1);
            stack += _emitter.Emit(OpCodeType.Stfld);
        }

        return stack.AreEqual(1);
    }

    protected internal override int VisitMemberInit(MemberInitExpression init)
    {
        var stack = init.New.Accept(this).AreEqual(1);
        foreach (var member in init.Members)
        {
            stack += _emitter.Emit(OpCodeType.Dup);
            stack += _emitter.Emit(OpCodeType.Ldstr, member.Key);
            stack += member.Value.Accept(this);
            stack += _emitter.Emit(OpCodeType.Stfld);
        }

        return stack.AreEqual(1);
    }

    protected internal override int VisitTry(TryExpression expr)
    {
        using (_emitter.Scope())
        {
            var tryEnd = _emitter.CreateLabel("try");
            var cacheLabel = _emitter.CreateLabel("catch");
            var finallyLabel = _emitter.CreateLabel("finally");
            var stack = 0;
            stack += _emitter.Emit(OpCodeType.LdLabel, cacheLabel.Target);
            stack += _emitter.Emit(OpCodeType.LdLabel, finallyLabel.Target);
            stack += _emitter.Emit(OpCodeType.Try);
            stack += expr.Try.Accept(this);
            stack += _emitter.Emit(OpCodeType.EndTry);
            stack += _emitter.Emit(OpCodeType.Br, finallyLabel.Target);
            _emitter.BindLabel(cacheLabel);
            stack += expr.Catch?.Accept(this) ?? 0;
            _emitter.BindLabel(finallyLabel);
            stack += expr.Finally.Accept(this);
            _emitter.BindLabel(tryEnd);
            return stack.AreEqual(0);
        }
    }

    protected internal override int VisitCatch(CatchExpression expr)
    {
        using (_emitter.Scope())
        {
            var label = _emitter.GetLabel("finally");
            var stack = 0;
            stack += _emitter.Emit(OpCodeType.Catch);
            if (expr.Variable != null)
            {
                var variable = _emitter.CreateVariable(expr.Variable.Name);
                if (expr.Variable.Value != null)
                {
                    stack += expr.Variable.Value.Accept(this).AreEqual(1);
                    stack += _emitter.Emit(OpCodeType.Stloc, variable);
                }
            }

            stack += expr.Block.Accept(this);
            stack += _emitter.Emit(OpCodeType.EndCatch);
            stack += _emitter.Emit(OpCodeType.Br, label.Target);
            return stack.AreEqual(0);
        }
    }

    protected internal override int VisitFinally(FinallyExpression expr)
    {
        using (_emitter.Scope())
        {
            var label = _emitter.GetLabel("try");
            var stack = 0;
            stack += _emitter.Emit(OpCodeType.Finally);
            stack += expr.Block.Accept(this);
            stack += _emitter.Emit(OpCodeType.EndFinally);
            stack += _emitter.Emit(OpCodeType.Br, label.Target);
            return stack.AreEqual(0);
        }
    }

    protected internal override int VisitThrow(ThrowExpression expr)
    {
        var varaibale = _emitter.GetVariable("%catch%");
        var stack = expr.Expression.Accept(this).AreEqual(0);
        stack += _emitter.Emit(OpCodeType.Ldloc, varaibale);
        stack += _emitter.Emit(OpCodeType.Throw);
        return stack.AreEqual(0);
    }
}

internal static class StackExtensions
{
    public static int AreEqual(this int stack, int want)
    {
        if (stack == want)
        {
            return stack;
        }

        return stack;
        throw new Exception($"stack:{stack}!= want:{want}");
    }
}