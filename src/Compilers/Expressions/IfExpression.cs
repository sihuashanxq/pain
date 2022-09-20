using System.Text;

namespace Pain.Compilers.Expressions;

public class IfExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.If;

    public Syntax Test { get; }

    public Syntax IfTrue { get; }

    public Syntax IfFalse { get; }

    public IfExpression(Syntax test, Syntax ifTrue, Syntax ifFalse)
    {
        Test = test;
        IfTrue = ifTrue;
        IfFalse = ifFalse;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitIf(this);
    }
}