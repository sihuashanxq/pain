using Pain.Compilers.Expressions;

namespace Pain.Compilers.Parsers.Rewriters;

public class Scope
{
    public Scope? Parent { get; private set; }

    public FunctionExpression Function { get; private set; }

    public Dictionary<string, object> Names { get; private set; }

    public HashSet<object> Captures { get; private set; }

    public Scope(FunctionExpression function)
    {
        Names = new Dictionary<string, object>();
        Function = function;
        Captures = new HashSet<object>();
    }

    public IDisposable Enter()
    {
        return Enter(Function);
    }

    public IDisposable Enter(FunctionExpression function)
    {
        var scope = new Scope(Function)
        {
            Names = Names,
            Parent = Parent,
            Captures = Captures
        };

        Names = new Dictionary<string, object>();
        Parent = scope;
        Function = function;

        return new Disposable(() =>
        {
            Names = Parent?.Names!;
            Parent = Parent?.Parent!;
            Function = Parent?.Function!;
        });
    }

    public void AddName(string name, object syntax)
    {
        if (!string.IsNullOrEmpty(name))
        {
            Names[name] = syntax;
        }
    }

    public bool Capture(NameExpression expr, FunctionExpression function)
    {
        if (Names.TryGetValue(expr.Name, out var v))
        {
            if (Function == function)
            {
                return false;
            }

            switch (v)
            {
                case FunctionExpression item:
                    Captures.Add(item);
                    Captures.Add(expr);
                    return true;
                case ParameterExpression item:
                    Captures.Add(item);
                    Captures.Add(expr);
                    return true;
                case VaraibleDefinition item:
                    Captures.Add(item);
                    Captures.Add(expr);
                    return true;
                default:
                    return false;
            }
        }

        if (Parent != null)
        {
            return Parent.Capture(expr, function);
        }

        return false;
    }
}

public class ScopedSyntaxWalker : SyntaxVisitor<Syntax>
{
    private Scope _scope;

    private FunctionExpression _function;

    public ScopedSyntaxWalker(FunctionExpression function)
    {
        _scope = new Scope(function);
        _function = function;
    }

    public HashSet<object> Walk()
    {
        Visit(_function);
        return _scope.Captures;
    }

    protected internal override Syntax VisitBinary(BinaryExpression expr)
    {
        expr.Left.Accept(this);
        expr.Right.Accept(this);
        return expr;
    }

    protected internal override Syntax VisitBlock(BlockExpression expr)
    {
        foreach (var item in expr.Statements)
        {
            item.Accept(this);
        }
        return expr;
    }

    protected internal override Syntax VisitBreak(BreakExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitCall(CallExpression expr)
    {
        expr.Function.Accept(this);
        expr.Arguments.ForEach(item => item.Accept(this));
        return expr;
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
        using (_scope.Enter())
        {
            expr.Initializers?.ForEach(item => item.Accept(this));
            expr.Test?.Accept(this);
            expr.Body?.Accept(this);
            expr.Iterators?.ForEach(item => item.Accept(this));
            return expr;
        }
    }

    protected internal override Syntax VisitFunction(FunctionExpression expr)
    {
        if (expr != _function)
        {
            _scope.AddName(expr.Name, expr);
        }

        using (_scope.Enter(expr))
        {
            expr.Parameters.ForEach(item => item.Accept(this));
            expr.Body.Accept(this);
        }

        return expr;
    }

    protected internal override Syntax VisitIf(IfExpression expr)
    {
        using (_scope.Enter())
        {
            expr.Test?.Accept(this);
            expr.IfTrue?.Accept(this);
            expr.IfFalse?.Accept(this);

            return expr;
        }
    }

    protected internal override Syntax VisitJSONArray(JSONArrayExpression expr)
    {
        expr.Items.ForEach(item => item.Accept(this));
        return expr;
    }

    protected internal override Syntax VisitJSONObject(JSONObjectExpression expr)
    {
        expr.Fields.Values.ForEach(item => item.Accept(this));
        return expr;
    }

    protected internal override Syntax VisitMember(MemberExpression expr)
    {
        expr.Object.Accept(this);
        expr.Member.Accept(this);
        return expr;
    }

    protected internal override Syntax VisitName(NameExpression expr)
    {
        _scope.Capture(expr, _scope.Function);
        return expr;
    }

    protected internal override Syntax VisitNew(NewExpression expr)
    {
        expr.Class.Accept(this);
        expr.Arguments.ForEach(item => item.Accept(this));
        return expr;
    }

    protected internal override Syntax VisitParameter(ParameterExpression expr)
    {
        _scope.AddName(expr.Name, expr);
        return expr;
    }

    protected internal override Syntax VisitReturn(ReturnExpression expr)
    {
        expr.Value?.Accept(this);
        return expr;
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
        expr.Expression.Accept(this);
        return expr;
    }

    protected internal override Syntax VisitVariable(VariableExpression expr)
    {
        foreach (var item in expr.Varaibles)
        {
            item.Value?.Accept(this);
            _scope.AddName(item.Name, item);
        }

        return expr;
    }
}

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> handler)
    {
        foreach (var item in items)
        {
            handler(item);
        }
    }
}