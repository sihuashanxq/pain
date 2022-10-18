namespace Pain.Compilers.Expressions;

public class JSONObjectExpression : Syntax
{
    public override SyntaxType Type=>SyntaxType.JSONObject;

    public Dictionary<string, Syntax> Fields { get; }

    public JSONObjectExpression(Dictionary<string,Syntax> fields)
    {
        Fields=fields;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitJSONObject(this);
    }
}