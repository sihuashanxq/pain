namespace Pain.Compilers.Expressions;

public class BinaryExpression:Syntax
{
    public override SyntaxType Type { get; }

    public Syntax Left { get; }

    public Syntax Right { get; }

    public BinaryExpression(Syntax left, Syntax right, SyntaxType type)
    {
        Type = type;
        Left = left;
        Right = right;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitBinary(this);
    }

    public override string ToString()
    {

        return $"{Left} {Type} {Right}";
    }
}