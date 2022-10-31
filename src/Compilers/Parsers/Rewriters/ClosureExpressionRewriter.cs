using Pain.Compilers.Expressions;
using Pain.Compilers.Parsers.Definitions;
namespace Pain.Compilers.Parsers.Rewriters;

public class ClosureExpressionRewriter : SyntaxVisitor<Syntax>
{
    private ModuleDefinition _module;

    private FunctionExpression _function;

    public ClosureExpressionRewriter(FunctionExpression function, ModuleDefinition module)
    {
        _module = module;
        _function = function;
    }

    public Syntax Rewrite()
    {
        return Visit(_function);
    }

    protected internal override Syntax VisitBinary(BinaryExpression expr)
    {
        return Syntax.MakeBinary(expr.Left.Accept(this), expr.Right.Accept(this), expr.Type);
    }

    protected internal override Syntax VisitBlock(BlockExpression expr)
    {
        return Syntax.MakeBlock(expr.Statements.Select(i => i.Accept(this)).ToArray());
    }

    protected internal override Syntax VisitBreak(BreakExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitCall(CallExpression expr)
    {
        return Syntax.MakeCall(expr.Function.Accept(this), expr.Arguments.Select(i => i.Accept(this)).ToArray());
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
        if (expr.IsLocal)
        {
            var rewriter = new ClosureExpressionRewriter(expr, _module);
            var name = new NameExpression(expr.Name);
            var @new = Syntax.MakeNew(name, Array.Empty<Syntax>());
            var members = new Dictionary<string, Syntax>();
            rewriter.VisitFunctionChildren(expr);

            foreach (var item in expr.CaptureVariables)
            {
                if (_function.CapturedVariables.TryGetValue(item.Value, out var _))
                {
                    members[item.Key.Name] = Syntax.MakeName(item.Key.Name);
                }
                else
                {
                    members[item.Key.Name] = Syntax.MakeMember(Syntax.MakeThis(), Syntax.MakeName(item.Key.Name));
                }
            }

            _module.Classes.Add(new ClassDefinition(name.Name, string.Empty, new[] { expr }));
            return Syntax.MakeMember(Syntax.MakeMemberInit(@new, members), Syntax.MakeName(name.Name));
        }

        return VisitFunctionChildren(expr);
    }

    protected internal FunctionExpression VisitFunctionChildren(FunctionExpression expr)
    {
        expr.Body = expr.Body.Accept(this);
        return expr;
    }

    protected internal override Syntax VisitIf(IfExpression expr)
    {
        return Syntax.MakeIf(expr.Test?.Accept(this)!, expr.IfTrue?.Accept(this)!, expr.IfFalse?.Accept(this)!);
    }

    protected internal override Syntax VisitArrayInit(ArrayInitExpression expr)
    {
        var items = new List<Syntax>();
        foreach (var item in expr.Items)
        {
            items.Add(item.Accept(this));
        }

        return Syntax.MakeArray(items.ToArray());
    }

    protected internal override Syntax VisitMember(MemberExpression expr)
    {
        return Syntax.MakeMember(expr.Object.Accept(this), expr.Member.Accept(this));
    }

    protected internal override Syntax VisitName(NameExpression expr)
    {
        return UnWrap(expr);
    }

    protected internal override Syntax VisitNew(NewExpression expr)
    {
        return Syntax.MakeNew(expr.Class.Accept(this), expr.Arguments.Select(i => i.Accept(this)).ToArray());
    }

    protected internal override Syntax VisitParameter(ParameterExpression expr)
    {
        return expr;
    }

    protected internal override Syntax VisitReturn(ReturnExpression expr)
    {
        return Syntax.MakeReturn(expr.Value?.Accept(this)!);
    }

    protected internal override Syntax VisitSuper(SuperExpression expr)
    {
        if (_function.IsLocal)
        {
            return UnWrap(expr);
        }

        return expr;
    }

    protected internal override Syntax VisitThis(ThisExpression expr)
    {
        if (_function.IsLocal)
        {
            return UnWrap(expr);
        }

        return expr;
    }

    protected internal override Syntax VisitUnary(UnaryExpression expr)
    {
        return Syntax.MakeUnary(expr.Expression.Accept(this), expr.Type);
    }

    protected internal override Syntax VisitVariable(VariableExpression expr)
    {
        var items = new List<Varaible>();
        foreach (var item in expr.Varaibles)
        {
            items.Add(Wrap(item));
        }

        return Syntax.MakeVariable(items.ToArray());
    }

    private Syntax Wrap(Syntax value, string name)
    {
        return new MemberInitExpression(Syntax.MakeNew(Syntax.MakeConstant("Runtime.Object", SyntaxType.ConstString), Array.Empty<Syntax>()), (new Dictionary<string, Syntax> { [name] = value }));
    }

    private Varaible Wrap(Varaible item)
    {
        if (_function.CapturedVariables.Contains(item))
        {
            return new Varaible(item.Name, Wrap(item.Value?.Accept(this)!, item.Name));
        }
        else
        {
            return new Varaible(item.Name, item.Value?.Accept(this)!);
        }
    }

    private Syntax UnWrap(NameExpression name)
    {
        if (_function.CaptureVariables.TryGetValue(name, out var _))
        {
            if (!_function.IsLocal)
            {
                return Syntax.MakeMember(name, Syntax.MakeConstant(name.Name, SyntaxType.ConstString));
            }

            return Syntax.MakeMember(
                Syntax.MakeMember(
                    Syntax.MakeThis(),
                    Syntax.MakeConstant(name, SyntaxType.ConstString)),
                Syntax.MakeConstant(name.Name, SyntaxType.ConstString));
        }

        if (_function.CapturedVariables.Contains(name))
        {
            return Syntax.MakeMember(name, Syntax.MakeConstant(name.Name, SyntaxType.ConstString));
        }

        return name;
    }

    private Syntax UnWrap(ThisExpression expr)
    {
        if (_function.CaptureVariables.TryGetValue(expr, out var _))
        {
            return Syntax.MakeMember(Syntax.MakeName(expr.Name), Syntax.MakeConstant(expr.Name, SyntaxType.ConstString));
        }

        if (_function.CapturedVariables.Contains(expr))
        {
            return Syntax.MakeMember(Syntax.MakeName(expr.Name), Syntax.MakeConstant(expr.Name, SyntaxType.ConstString));
        }

        return expr as Syntax;
    }

    private Syntax UnWrap(SuperExpression expr)
    {
        if (_function.CaptureVariables.TryGetValue(expr, out var _))
        {
            return Syntax.MakeMember(Syntax.MakeName("this"), Syntax.MakeConstant(expr.Name, SyntaxType.ConstString));
        }

        if (_function.CapturedVariables.Contains(expr))
        {
            return Syntax.MakeMember(Syntax.MakeName("this"), Syntax.MakeConstant(expr.Name, SyntaxType.ConstString));
        }

        return expr as Syntax;
    }

    protected internal override Syntax VisitMemberInit(MemberInitExpression init)
    {
        var @new = init.New.Accept(this);
        var members = new Dictionary<string, Syntax>();

        foreach (var member in init.Members)
        {
            members[member.Key] = member.Value.Accept(this);
        }

        return Syntax.MakeMemberInit(@new, members);
    }
}