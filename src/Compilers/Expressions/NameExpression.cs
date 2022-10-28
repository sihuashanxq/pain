namespace Pain.Compilers.Expressions;


public class NameExpression : Syntax, ICaptureable
{
    public override SyntaxType Type => SyntaxType.Name;

    public string Name { get; }

    public NameExpression(string name)
    {
        Name = name;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitName(this);
    }

    public override string ToString()
    {
        return Name;
    }
}