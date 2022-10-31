namespace Pain.Compilers.Expressions;

public class ArrayInitExpression : Syntax
{
    public Syntax[] Items { get; }

    public override SyntaxType Type => SyntaxType.ArrayInit;

    public ArrayInitExpression(Syntax[] items)
    {
        Items = items;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitArrayInit(this);
    }

    public override string ToString()
    {
        return $"new [{string.Join(",\n", Items.Select(i => i.ToString()))}]";
    }
}