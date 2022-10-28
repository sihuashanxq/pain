namespace Pain.Compilers.Expressions;

public class JSONObjectExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.JSONObject;

    public Dictionary<string, Syntax> Fields { get; }

    public JSONObjectExpression(Dictionary<string, Syntax> fields)
    {
        Fields = fields;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitJSONObject(this);
    }

    public override string ToString()
    {
        return $"new {{{string.Join(",\n", Fields.Select(i => string.Format("\"{0}\":{1}", i.Key, i.Value.ToString())))}}}";
    }
}