namespace Pain.Compilers.Expressions;

public class SuperExpression : Syntax
{
    public string Name => "super";

    public override SyntaxType Type => SyntaxType.Super;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitSuper(this);
    }

    public override string ToString()
    {
        return $"super ";
    }
}