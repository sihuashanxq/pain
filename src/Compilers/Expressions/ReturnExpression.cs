namespace Pain.Compilers.Expressions;

public class ReturnExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Member;

    public Syntax Value { get; }

    public ReturnExpression(Syntax value)
    {
        Value = value;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitReturn(this);
    }
}