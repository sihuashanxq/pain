namespace Pain.Compilers.Expressions;

public class ContinueExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Continue;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitContinue(this);
    }

    public override string ToString()
    {
        return "continue";
    }
}