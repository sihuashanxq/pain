namespace Pain.Compilers.Expressions;

public class ThisExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.This;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitThis(this);
    }
}