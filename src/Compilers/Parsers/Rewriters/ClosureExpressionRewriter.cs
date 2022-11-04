using Pain.Compilers.Expressions;
using Pain.Compilers.Parsers.Definitions;
namespace Pain.Compilers.Parsers.Rewriters;

public class ClosureExpressionRewriter : SyntaxVisitor<Syntax>
{
    private readonly ClassDefinition _class;

    private readonly ModuleDefinition _module;

    private readonly FunctionExpression _function;

    private readonly Syntax _bind;

    public ClosureExpressionRewriter(FunctionExpression function, ModuleDefinition module, ClassDefinition @class)
    {
        _class = @class;
        _module = module;
        _function = function;
        _bind = Syntax.MakeConstant("bind", SyntaxType.ConstString);
    }

    public FunctionExpression? Rewrite()
    {
        ScopedSyntaxWalker.Walk(_function);
        return Visit(_function) as FunctionExpression;
    }

    protected internal override Syntax VisitBinary(BinaryExpression expr)
    {
        if (expr.Type == SyntaxType.Assign && !_function.IsLocal)
        {

        }
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
        if(expr.Function.Type == SyntaxType.Super)
        {
            return Syntax.MakeCall(
                Syntax.MakeCall(
                    Syntax.MakeMember(
                        Syntax.MakeMember(
                            Syntax.MakeName(_class.Super),
                            Syntax.MakeConstant("ctor", SyntaxType.ConstString)
                        ),
                    Syntax.MakeConstant("bind", SyntaxType.ConstString)
                    ),
                    new[] { Syntax.MakeThis() }
                 ),
                    expr.Arguments.Select(i => i.Accept(this)).ToArray()
                );
        }
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
            expr.Iterators?.Select(i => i.Accept(this)).ToArray()!,
            expr.Body?.Accept(this)!
        );
    }

    protected internal override Syntax VisitFunction(FunctionExpression expr)
    {
        if (expr.IsLocal)
        {
            var rewriter = new ClosureExpressionRewriter(expr, _module, _class);
            var name = new NameExpression(expr.Name);
            var @new = Syntax.MakeNew(name, Array.Empty<Syntax>());
            var members = new Dictionary<string, Syntax>();
            var newClass = new ClassDefinition(name.Name, "Runtime.Object");
            rewriter.VisitFunctionChildren(expr);
            newClass.AddFunction(expr);

            foreach (var item in expr.CaptureVariables)
            {
                if (_function.CapturedVariables.TryGetValue(item.Value, out var _))
                {
                    members[item.Key.Name] = Syntax.MakeName(item.Key.Name);
                }
                else
                {
                    members[item.Key.Name] = Syntax.MakeMember(Syntax.MakeThis(), Syntax.MakeConstant(name.Name, SyntaxType.ConstString));
                }
            }

            _module.Classes.Add(newClass);
            return Syntax.MakeMember(Syntax.MakeMemberInit(@new, members), Syntax.MakeConstant(name.Name, SyntaxType.ConstString));
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
        if (expr.Object.Type != SyntaxType.Super)
        {
            return Syntax.MakeMember(expr.Object.Accept(this), expr.Member.Accept(this));
        }

        if (!_function.IsLocal)
        {
            return Syntax.MakeCall(
               Syntax.MakeMember(Syntax.MakeMember(expr.Object.Accept(this), expr.Member.Accept(this)), _bind),
               new[] { Syntax.MakeThis() }
            );
        }

        return Syntax.MakeCall(
            Syntax.MakeMember(Syntax.MakeMember(expr.Object.Accept(this), expr.Member.Accept(this)), _bind),
            new[] { Syntax.MakeMember(Syntax.MakeThis(), Syntax.MakeConstant(Syntax.MakeThis().Name, SyntaxType.ConstString)) }
        );

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
        return Syntax.MakeName(_class.Super);
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
        return new MemberInitExpression(Syntax.MakeNew(Syntax.MakeName("Object"), Array.Empty<Syntax>()), (new Dictionary<string, Syntax> { [name] = value }));
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
            return Syntax.MakeMember(Syntax.MakeThis(), Syntax.MakeConstant(expr.Name, SyntaxType.ConstString));
        }

        if (_function.CapturedVariables.Contains(expr))
        {
            return Syntax.MakeMember(Syntax.MakeThis(), Syntax.MakeConstant(expr.Name, SyntaxType.ConstString));
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