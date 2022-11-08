namespace Pain.Compilers.Expressions;

public class TryExpression : Syntax
{
    public Syntax Try { get; }

    public Syntax? Catch { get; }

    public Syntax Finally { get; }

    public TryExpression(Syntax tryExpr, Syntax? catchExpr, Syntax finallyExpr)
    {
        Try = tryExpr;
        Catch = catchExpr;
        Finally = finallyExpr;
    }

    public override SyntaxType Type => throw new NotImplementedException();

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }
}

public class FinallyExpression : Syntax
{
    public Syntax Block { get; }

    public FinallyExpression(Syntax block)
    {
        Block = block;
    }

    public override SyntaxType Type => throw new NotImplementedException();

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        throw new NotImplementedException();
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

    public override SyntaxType Type => throw new NotImplementedException();

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }
}