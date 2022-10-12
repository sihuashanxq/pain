
using Pain.Compilers.Expressions;

namespace Pain.Compilers.CodeGen;

public class FunctionCompiler : Expressions.SyntaxVisitor<int>
{
    protected internal override int VisitBinary(BinaryExpression binaryExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitBlock(BlockExpression blockExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitBreak(BreakExpression breakExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitCall(CallExpression callExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitConstant(ConstantExpression constantExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitContinue(ContinueExpression continueExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitEmpty(EmptyExpression emptyExpression)
    {
        return 0;
    }

    protected internal override int VisitFor(ForExpression forExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitFunction(FunctionExpression functionExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitIf(IfExpression ifExpression)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    protected internal override int VisitParameter(ParameterExpression parameterExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitReturn(ReturnExpression returnExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitSuper(SuperExpression superExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitThis(ThisExpression thisExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override int VisitUnary(UnaryExpression unaryExpression)
    {
        throw new NotImplementedException();
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