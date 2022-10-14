using Pain.Compilers.Expressions;

namespace Pain.Compilers.Parsers.Rewriters;

public class Scope
{
    public Scope? Parent { get; private set; }

    public FunctionExpression Function { get; private set; }

    public Dictionary<string, Syntax> Names { get; private set; }

    public Scope(FunctionExpression function)
    {
        Names = new Dictionary<string, Syntax>();
        Function = function;
    }

    public void Enter(FunctionExpression function)
    {
        var scope = new Scope(Function)
        {
            Names = Names,
            Parent = Parent
        };

        Names = new Dictionary<string, Syntax>();
        Parent = scope;
        Function = function;
    }

    public void Exit()
    {
        Names = Parent!.Names;
        Parent = Parent!.Parent;
        Function = Parent!.Function;
    }

    public void AddName(string name, Syntax syntax)
    {
        if (!string.IsNullOrEmpty(name))
        {
            Names[name] = syntax;
        }
    }

    public bool Captured(string name, FunctionExpression function)
    {
        if (Names.TryGetValue(name, out var v))
        {
            if (Function == function)
            {
                return false;
            }

            switch (v)
            {
                case FunctionExpression f:
                    f.Captured = true;
                    return true;
                case ParameterExpression p:
                    p.Captured = true;
                    return true;
                case VariableExpression var:
                    return true;
                default:
                    throw new Exception("unknown name syntax type");
            }
        }

        if (Parent != null)
        {
            return Parent.Captured(name, function);
        }

        return false;
    }
}

public class ScopedSyntaxWalker : SyntaxVisitor<Syntax>
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