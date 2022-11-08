using Pain.Compilers.Expressions;

namespace Pain.Compilers.Parsers.Rewriters;

public class Scope
{
    public Scope? Parent { get; private set; }

    public FunctionExpression Function { get; private set; }

    public Dictionary<string, ICaptureable> Names { get; private set; }

    public Scope(FunctionExpression function)
    {
        Names = new Dictionary<string, ICaptureable>();
        Function = function;
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
        };

        Names = new Dictionary<string, ICaptureable>();
        Parent = scope;
        Function = function;

        return new Disposable(() =>
        {
            Names = Parent?.Names!;
            Parent = Parent?.Parent!;
            Function = Parent?.Function!;
        });
    }

    public void AddVaribale(Variable var)
    {
        Names[var.Name] = var;
    }

    public void AddThis()
    {
        var expr = Syntax.MakeThis();
        Names[expr.Name] = expr;
    }

    public bool Capture(ICaptureable expr, FunctionExpression function)
    {
        if (Names.TryGetValue(expr.Name, out var v))
        {
            if (Function == function)
            {
                return false;
            }

            Function.CapturedVariables.Add(v);
            function.CaptureVariables[expr] = v;
            return true;
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
    private readonly Scope _scope;

    private readonly FunctionExpression _function;

    public ScopedSyntaxWalker(FunctionExpression function) : this(function, new Scope(function))
    {

    }

    public ScopedSyntaxWalker(FunctionExpression function, Scope scope)
    {
        _scope = scope;
        _function = function;
    }

    public void Walk()
    {
        if (!_function.Native)
        {
            Visit(_function);
        }
    }

    public static void Walk(FunctionExpression function)
    {
        var walker = new ScopedSyntaxWalker(function);
        walker.Walk();
    }

    public static Dictionary<ICaptureable, ICaptureable> Walk(FunctionExpression expr, Scope scope)
    {
        var walker = new ScopedSyntaxWalker(expr, scope);
        walker.Walk();
        return expr.CaptureVariables;
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
            foreach (var item in Walk(expr, _scope))
            {
                if (!_function.CapturedVariables.Contains(item.Value))
                {
                    _function.CaptureVariables[item.Key] = item.Value;
                }
            }
            return expr;
        }

        using (_scope.Enter(expr))
        {
            if (!expr.Local)
            {
                _scope.AddThis();
            }

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

    protected internal override Syntax VisitArrayInit(ArrayInitExpression expr)
    {
        expr.Items.ForEach(item => item.Accept(this));
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
        if (_function.Local)
        {
            _scope.Capture(expr, _function);
        }

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
            _scope.AddVaribale(item);
        }

        return expr;
    }

    protected internal override Syntax VisitMemberInit(MemberInitExpression init)
    {
        init.New.Accept(this);
        init.Members.Select(i => i.Value.Accept(this));
        return init;
    }

    protected internal override Syntax VisitTry(TryExpression expr)
    {
        using (_scope.Enter())
        {
            expr.Try.Accept(this);
            expr.Catch?.Accept(this);
            expr.Finally.Accept(this);
            return expr;
        }
    }

    protected internal override Syntax VisitCatch(CatchExpression expr)
    {
        using (_scope.Enter())
        {
            if (expr.Variable != null)
            {
                _scope.AddVaribale(expr.Variable);
            }

            expr.Block.Accept(this);
            return expr;
        }
    }

    protected internal override Syntax VisitFinally(FinallyExpression expr)
    {
        using (_scope.Enter())
        {
            expr.Block.Accept(this);
            return expr;
        }
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