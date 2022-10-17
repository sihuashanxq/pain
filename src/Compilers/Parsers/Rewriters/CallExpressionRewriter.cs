using Pain.Compilers.Expressions;
namespace Pain.Compilers.Parsers.Rewriters;

public class CallExpressionRewriter : SyntaxVisitor<Syntax>
{
    private FunctionExpression _function;

    public CallExpressionRewriter(FunctionExpression function)
    {
        _function = function;
    }

    protected internal override Syntax VisitBinary(BinaryExpression expression)
    {
        var left = expression.Left.Accept(this);
        var right = expression.Right.Accept(this);
        return new BinaryExpression(left, right, expression.Type);
    }

    protected internal override Syntax VisitBlock(BlockExpression expression)
    {
        return new BlockExpression(expression.Statements.Select(i => i.Accept(this)).ToArray());
    }

    protected internal override Syntax VisitBreak(BreakExpression expression)
    {
        return expression;
    }

    protected internal override Syntax VisitCall(CallExpression expression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitConstant(ConstantExpression expression)
    {
        return expression;
    }

    protected internal override Syntax VisitContinue(ContinueExpression expression)
    {
        return expression;
    }

    protected internal override Syntax VisitEmpty(EmptyExpression expression)
    {
        return expression;
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

    protected internal override Syntax VisitName(NameExpression expression)
    {
        return expression;
    }

    protected internal override Syntax VisitNew(NewExpression newExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitParameter(ParameterExpression expression)
    {
        return expression;
    }

    protected internal override Syntax VisitReturn(ReturnExpression expression)
    {
        return new ReturnExpression(expression.Value?.Accept(this)!);
    }

    protected internal override Syntax VisitSuper(SuperExpression expression)
    {
      return expression;
    }

    protected internal override Syntax VisitThis(ThisExpression expression)
    {
        return expression;
    }

    protected internal override Syntax VisitUnary(UnaryExpression expression)
    {
        return new UnaryExpression(expression.Expression.Accept(this), expression.Type);
    }

    protected internal override Syntax VisitVariable(VariableExpression variablExpression)
    {
        return variablExpression;
    }
}