namespace Pain.Compilers.Expressions;

public class ThisExpression : Syntax, ICaptureable
{
    public string Name => "this";

    public override SyntaxType Type => SyntaxType.This;

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitThis(this);
    }

    public override string ToString()
    {
        return $" this ";
    }
}