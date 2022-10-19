using Pain.Compilers.Expressions;

namespace Pain.Compilers.Parsers.Rewriters;

public class ClosureExpressionRewriter : SyntaxVisitor<Syntax>
{
    protected internal override Syntax VisitBinary(BinaryExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitBlock(BlockExpression expr)
    {
        return new BlockExpression(expr.Statements.Select(i => i.Accept(this)).ToArray());
    }

    protected internal override Syntax VisitBreak(BreakExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitCall(CallExpression expr)
    {
        return new CallExpression(expr.Function.Accept(this), expr.Arguments.Select(i => i.Accept(this)).ToArray());
    }

    protected internal override Syntax VisitConstant(ConstantExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitContinue(ContinueExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitEmpty(EmptyExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitFor(ForExpression expr)
    {
        return new ForExpression(
            expr.Initializers?.Select(i => i.Accept(this)).ToArray()!,
            expr.Test?.Accept(this)!,
            expr.Initializers?.Select(i => i.Accept(this)).ToArray()!,
            expr.Body?.Accept(this)!
        );
    }

    protected internal override Syntax VisitFunction(FunctionExpression expr)
    {
        if (!expr.Captured)
        {
            return expr;
        }

        var body = new List<Syntax>();

        foreach (var parameter in expr.Parameters)
        {
            if (parameter.Captured)
            {
                body.Add(
                    new BinaryExpression(
                        new NameExpression(parameter.Name),
                        new JSONObjectExpression(
                            new Dictionary<string, Syntax>
                            {
                                ["value"] = expr
                            }
                        ),
                        SyntaxType.Assign
                    )
                );
            }
        }

        return new JSONObjectExpression(new Dictionary<string, Syntax>
        {
            ["value"] = expr
        });
    }

    protected internal override Syntax VisitIf(IfExpression expr)
    {
        return new IfExpression(expr.Test?.Accept(this)!, expr.IfTrue?.Accept(this)!, expr.IfFalse?.Accept(this)!);
    }

    protected internal override Syntax VisitJSONArray(JSONArrayExpression expression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitJSONObject(JSONObjectExpression expression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitMember(MemberExpression memberExpression)
    {
        throw new NotImplementedException();
    }

    protected internal override Syntax VisitName(NameExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitNew(NewExpression expr)
    {
        return new NewExpression(expr.Class.Accept(this), expr.Arguments.Select(i => i.Accept(this)).ToArray());
    }

    protected internal override Syntax VisitParameter(ParameterExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitReturn(ReturnExpression expr)
    {
        return new ReturnExpression(expr.Value?.Accept(this)!);
    }

    protected internal override Syntax VisitSuper(SuperExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitThis(ThisExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitUnary(UnaryExpression expr)
    {
        return new UnaryExpression(expr.Expression.Accept(this), expr.Type);
    }

    protected internal override Syntax VisitVariable(VariableExpression expr)
    {
        return expr;
    }

    private Syntax Capture(Syntax value)
    {
        return new JSONObjectExpression(new Dictionary<string, Syntax> { [Constant.CapturedField] = value });
    }

    private Syntax UnCapture(Syntax value)
    {
        return new MemberExpression(value, new ConstantExpression(Constant.CapturedField, SyntaxType.ConstString));
    }
}

public static class Constant
{
    public const string CapturedField = "value";
}