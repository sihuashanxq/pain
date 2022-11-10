namespace Pain.Compilers.Expressions;

public class TryExpression : Syntax
{
    public Syntax Try { get; }

    public Syntax Catch { get; }

    public Syntax Finally { get; }

    public TryExpression(Syntax tryExpr, Syntax catchExpr, Syntax finallyExpr)
    {
        Try = tryExpr;
        Catch = catchExpr;
        Finally = finallyExpr;
    }

    public override SyntaxType Type => SyntaxType.Try;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitTry(this);
    }
}

public class FinallyExpression : Syntax
{
    public Syntax Block { get; }

    public FinallyExpression(Syntax block)
    {
        Block = block;
    }

    public override SyntaxType Type => SyntaxType.Finally;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitFinally(this);
    }
}
public class CatchExpression : Syntax
{
    public Syntax Block { get; }

    public Variable Variable { get; }

    public CatchExpression(Variable varaible, Syntax block)
    {
        Block = block;
        Variable = varaible;
    }

    public override SyntaxType Type => SyntaxType.Catch;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitCatch(this);
    }
}

public class ThrowExpression : Syntax
{
    public Syntax Expression { get; }

    public ThrowExpression(Syntax expr)
    {
        Expression = expr;
    }

    public override SyntaxType Type => SyntaxType.Throw;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitThrow(this);
    }
}