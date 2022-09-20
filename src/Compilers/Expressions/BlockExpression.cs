namespace Pain.Compilers.Expressions;

public class BlockExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Block;

    public Syntax[] Statements { get; }

    public BlockExpression(Syntax[] statements)
    {
        Statements = statements;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitBlock(this);
    }

    public override string ToString()
    {
        return $"{{\n{string.Join("\n", Statements.Select(i => i.ToString()))}\n}}";
    }
}

