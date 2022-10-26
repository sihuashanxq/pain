namespace Pain.Compilers.Expressions;

public class ParameterExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Parameter;

    public int Index { get; }

    public new string Name { get; }

    public ParameterExpression(string name, int index)
    {
        Name = name;
        Index = index;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitParameter(this);
    }
}