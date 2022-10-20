using Pain.Compilers.Expressions;

namespace Pain.Compilers.Parsers.Rewriters;

public class ClosureExpressionRewriter : SyntaxVisitor<Syntax>
{
    private HashSet<object> _catpures;

    private FunctionExpression _function;

    public ClosureExpressionRewriter(FunctionExpression function, HashSet<object> captures)
    {
        _catpures = captures;
        _function = function;
    }

    public Syntax Rewrite()
    {
        return Visit(_function);
    }

    protected internal override Syntax VisitBinary(BinaryExpression expr)
    {
        return new BinaryExpression(expr.Left.Accept(this), expr.Right.Accept(this), expr.Type);
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
        var body = new List<Syntax>();
        expr.Parameters.Where(item => IsCaptured(item)).ForEach(item =>
        {
            body.Add(Syntax.Binary(Syntax.Name(item.Name), Capture(item.Name), SyntaxType.Assign));
        });
        body.Add(expr.Body.Accept(this));
        expr = new FunctionExpression(expr.Name, expr.IsStatic, expr.IsNative, expr.IsConstructor, expr.Parameters, Syntax.Block(body.ToArray()));
        return IsCaptured(expr) ? Capture(expr) : expr;
    }

    protected internal override Syntax VisitIf(IfExpression expr)
    {
        return new IfExpression(expr.Test?.Accept(this)!, expr.IfTrue?.Accept(this)!, expr.IfFalse?.Accept(this)!);
    }

    protected internal override Syntax VisitJSONArray(JSONArrayExpression expr)
    {
        var items = new List<Syntax>();
        foreach (var item in expr.Items)
        {
            items.Add(item.Accept(this));
        }

        return new JSONArrayExpression(items.ToArray());
    }

    protected internal override Syntax VisitJSONObject(JSONObjectExpression expr)
    {
        var fields = new Dictionary<string, Syntax>();
        foreach (var kv in expr.Fields)
        {
            fields[kv.Key] = kv.Value.Accept(this);
        }

        return new JSONObjectExpression(fields);
    }

    protected internal override Syntax VisitMember(MemberExpression expr)
    {
        return Syntax.Member(expr.Object.Accept(this), expr.Member.Accept(this));
    }

    protected internal override Syntax VisitName(NameExpression expr)
    {
        return IsCaptured(expr) ? UnCapture(expr) : expr;
    }

    protected internal override Syntax VisitNew(NewExpression expr)
    {
        return Syntax.New(expr.Class.Accept(this), expr.Arguments.Select(i => i.Accept(this)).ToArray());
    }

    protected internal override Syntax VisitParameter(ParameterExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitReturn(ReturnExpression expr)
    {
        return Syntax.Return(expr.Value?.Accept(this)!);
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
        return Syntax.Unary(expr.Expression.Accept(this), expr.Type);
    }

    protected internal override Syntax VisitVariable(VariableExpression expr)
    {
        var items = new List<VaraibleDefinition>();
        foreach (var item in expr.Varaibles)
        {
            if (IsCaptured(item))
            {
                items.Add(new VaraibleDefinition(item.Name, Capture(item.Value!)));
            }
            else
            {
                items.Add(new VaraibleDefinition(item.Name, item.Value?.Accept(this)!));
            }
        }

        return new VariableExpression(items.ToArray());
    }

    private Syntax Capture(Syntax value)
    {
        return new JSONObjectExpression(new Dictionary<string, Syntax> { [Constant.CapturedField] = value });
    }

    private Syntax Capture(string name)
    {
        return Capture(new NameExpression(name));
    }

    private Syntax UnCapture(Syntax value)
    {
        return new MemberExpression(value, new ConstantExpression(Constant.CapturedField, SyntaxType.ConstString));
    }

    private bool IsCaptured(object expr)
    {
        return _catpures.Contains(expr);
    }
}

public static class Constant
{
    public const string CapturedField = "value";
}