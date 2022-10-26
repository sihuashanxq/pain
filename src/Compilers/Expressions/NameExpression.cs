namespace Pain.Compilers.Expressions;


public class NameExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Name;

    public new string Name { get; }

    public NameExpression(string name)
    {
        Name = name;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitName(this);
    }
}