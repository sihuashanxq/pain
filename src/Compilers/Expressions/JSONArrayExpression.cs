namespace Pain.Compilers.Expressions;

public class JSONArrayExpression : Syntax
{
    public Syntax[] Items { get; }

    public override SyntaxType Type => SyntaxType.JSONArray;

    public JSONArrayExpression(Syntax[] items)
    {
        Items = items;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitJSONArray(this);
    }
}