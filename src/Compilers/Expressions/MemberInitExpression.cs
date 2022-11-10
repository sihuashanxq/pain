namespace Pain.Compilers.Expressions;
public class MemberInitExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.MemberInit;

    public Syntax New { get; }

    public Dictionary<string, Syntax> Members { get; }

    public MemberInitExpression(Syntax @new, Dictionary<string, Syntax> members)
    {
        New = @new;
        Members = members;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitMemberInit(this);
    }

    public override string ToString()
    {
        return $"{New.ToString()} {{{string.Join(",\n", Members.Select(i => string.Format("\"{0}\":{1}", i.Key, i.Value.ToString())))}}}";
    }
}