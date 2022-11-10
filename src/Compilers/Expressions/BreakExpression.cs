namespace Pain.Compilers.Expressions;

public class BreakExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Break;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitBreak(this);
    }

    public override string ToString()
    {
        return "break";
    }
}