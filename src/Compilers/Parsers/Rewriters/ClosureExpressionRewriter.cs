using Pain.Compilers.Expressions;
namespace Pain.Compilers.Parsers.Rewriters;

public class ClosureExpressionRewriter : SyntaxVisitor<Syntax>
{
    protected internal override Syntax VisitBinary(BinaryExpression binaryExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitBlock(BlockExpression blockExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitBreak(BreakExpression breakExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitCall(CallExpression callExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitConstant(ConstantExpression constantExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitContinue(ContinueExpression continueExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitEmpty(EmptyExpression emptyExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitFor(ForExpression forExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitFunction(FunctionExpression functionExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitIf(IfExpression ifExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitMember(MemberExpression memberExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitName(NameExpression nameExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitNew(NewExpression newExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitParameter(ParameterExpression parameterExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitReturn(ReturnExpression returnExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitSuper(SuperExpression superExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitThis(ThisExpression thisExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitUnary(UnaryExpression unaryExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitVariable(VariableExpression variablExpression)
    {
        throw new NotImplementedException();
    }
}