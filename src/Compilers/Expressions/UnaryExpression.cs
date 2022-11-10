namespace Pain.Compilers.Expressions;

public class UnaryExpression : Syntax
{
    public override SyntaxType Type { get; }

    public Syntax Expression { get; }

    public UnaryExpression(Syntax expression, SyntaxType type)
    {
        Type = type;
        Expression = expression;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitUnary(this);
    }

    public override string ToString()
    {
        return $" {Type}{Expression} ";
    }
}