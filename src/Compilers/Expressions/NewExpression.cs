namespace Pain.Compilers.Expressions;

public class NewExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.New;

    public Syntax Class { get; }

    public Syntax[] Arguments { get; }

    public NewExpression(Syntax @class, Syntax[] arguments)
    {
        Class = @class;
        Arguments = arguments;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitNew(this);
    }

    public override string ToString()
    {
        return $"new {Class.ToString()}({string.Join(",", Arguments.Select(i => i.ToString()))})";
    }
}