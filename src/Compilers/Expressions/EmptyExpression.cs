namespace Pain.Compilers.Expressions;

public class EmptyExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Empty;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitEmpty(this);
    }

    public override string ToString()
    {
        return string.Empty;
    }
}