
using Pain.Compilers.Expressions;

namespace Pain.Compilers.CodeGen;

public class FunctionCompiler : Expressions.SyntaxVisitor<int>
{
    private readonly FunctionContext _function;

    private readonly FunctionEmitter _emitter;


    protected internal override int VisitBinary(BinaryExpression binaryExpression)
    {
        var stack = 0;

        /*
                    if (binaryExpression.Token.Type.IsAssign())
                    {
                        return VisitAssign(binaryExpression);
                    }
    
        */
        if (binaryExpression.Type == SyntaxType.Or)
        {
            using (_emitter.NewScope())
            {
                var next = _emitter.CreateLabel(Label.Next);

                stack += binaryExpression.Left.Accept(this);
                stack += _emitter.Emit(OpCodeType.Dup);
                stack += _emitter.Emit(OpCodeType.Brtrue, next.Target);
                stack += _emitter.Emit(OpCodeType.Pop);
                stack += binaryExpression.Right.Accept(this);

                _emitter.BindLabel(next);
                return stack;
            }
        }

        if (binaryExpression.Type == SyntaxType.And)
        {
            using (_emitter.NewScope())
            {
                var next = _emitter.CreateLabel(Label.Next);

                stack += binaryExpression.Left.Accept(this);
                stack += _emitter.Emit(OpCodeType.Dup);
                stack += _emitter.Emit(OpCodeType.Brfalse, next.Target);
                stack += _emitter.Emit(OpCodeType.Pop);
                stack += binaryExpression.Right.Accept(this);

                _emitter.BindLabel(next);
                return stack;
            }
        }

        if (binaryExpression.Type == SyntaxType.LessThan ||
            binaryExpression.Type == SyntaxType.LessThanOrEqual)
        {
            stack += binaryExpression.Right.Accept(this);
            stack += binaryExpression.Left.Accept(this);
        }
        else
        {
            stack += binaryExpression.Left.Accept(this);
            stack += binaryExpression.Right.Accept(this);
        }

        switch (binaryExpression.Type)
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
            case SyntaxType.LessThan:
            case SyntaxType.GreaterThan:
                stack += _emitter.Emit(OpCodeType.Gt);
                break;
            case SyntaxType.LessThanOrEqual:
            case SyntaxType.GreaterThanOrEqual:
                stack += _emitter.Emit(OpCodeType.Gte);
                break;
            default:
                throw new Exception($"not supported");
        }

        return stack;
    }

    protected internal override int VisitBlock(BlockExpression blockExpression)
    {
        using (_emitter.NewScope())
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

    protected internal override int VisitCall(CallExpression callExpression)
    {
        var stack = 0;

        foreach (var argument in callExpression.Arguments)
        {
            stack += argument.Accept(this);
        }

        stack += callExpression.Function.Accept(this);
        stack += _emitter.Emit(OpCodeType.Call, callExpression.Arguments.Length);

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
                stack += _emitter.Emit((bool)(constantExpression.Value) ? OpCodeType.Eq : OpCodeType.Gt);
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
        using (_emitter.NewScope())
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
            stack += _emitter.Emit(OpCodeType.Pop, stack);
            _emitter.BindLabel(next);

            stack += forExpression.Iterators.Sum(item => item.Accept(this)).AreEqual(0);
            stack += _emitter.Emit(OpCodeType.Br, begin.Target);
            _emitter.BindLabel(end);

            return stack.AreEqual(0);
        }
    }

    protected internal override int VisitFunction(FunctionExpression functionExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitIf(IfExpression ifExpression)
    {
        using (_emitter.NewScope())
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

            using (_emitter.NewScope())
            {
                _emitter.BindLabel(ifTrue);

                stack += ifExpression.IfTrue.Accept(this);
                stack += _emitter.Emit(OpCodeType.Pop, stack);
                stack += _emitter.Emit(OpCodeType.Br, ifEnd.Target);
            }

            if (ifExpression.IfFalse != null)
            {
                using (_emitter.NewScope())
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

    protected internal override int VisitMember(MemberExpression memberExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitName(NameExpression nameExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitNew(NewExpression newExpression)
    {
        var stack = 0;

        foreach (var argument in newExpression.Arguments)
        {
            stack = argument.Accept(this).AreEqual(1);
        }

        stack += newExpression.Class.Accept(this).AreEqual(1);
        stack += _emitter.Emit(OpCodeType.New, newExpression.Arguments.Length);

        return stack.AreEqual(1);
    }

    protected internal override int VisitParameter(ParameterExpression parameterExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitReturn(ReturnExpression returnExpression)
    {
        var stack = 0;

        if (returnExpression.Value != null)
        {
            stack = returnExpression.Value.Accept(this).AreEqual(1);
            stack += _emitter.Emit(OpCodeType.Ret);
        }
        else
        {
            stack += _emitter.Emit(OpCodeType.Ldnull);
            stack += _emitter.Emit(OpCodeType.Ret);
        }

        return stack.AreEqual(0);
    }

    protected internal override int VisitSuper(SuperExpression superExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitThis(ThisExpression thisExpression)
    {
        return _emitter.Emit(OpCodeType.Ldarg, 0).AreEqual(1);
    }

    protected internal override int VisitUnary(UnaryExpression unaryExpression)
    {
        var stack = unaryExpression.Expression.Accept(this);
        var tokenType = unaryExpression.Type;

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

    protected internal override int VisitVariable(VariableExpression variablExpression)
    {
        throw new NotImplementedException();
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

        throw new Exception($"stack:{stack}!= want:{want}");
    }
}