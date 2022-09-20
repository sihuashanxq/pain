namespace Pain.Compilers.Expressions;

public class MemberExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Member;

    public Syntax Object { get; }

    public Syntax Member { get; }

    public MemberExpression(Syntax @object, Syntax member)
    {
        Object = @object;
        Member = member;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitMember(this);
    }
}