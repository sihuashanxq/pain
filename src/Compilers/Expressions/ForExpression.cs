using System.Text;

namespace Pain.Compilers.Expressions;

public class ForExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.For;

    public Syntax Body { get; }

    public Syntax Test { get; }

    public Syntax[] Iterators { get; }

    public Syntax[] Initializers { get; }

    public ForExpression(Syntax[] initializers, Syntax test, Syntax[] iterators, Syntax body)
    {
        Test = test;
        Body = body;
        Iterators = iterators;
        Initializers = initializers;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitFor(this);
    }

    public override string ToString()
    {
        return new StringBuilder().
            Append("for ").
            Append(string.Join(",", Initializers?.Select(i => i.ToString())!)).
            Append(";").
            Append(Test?.ToString()!).
            Append(";").
            Append(string.Join(",", Iterators?.Select(i => i.ToString())!)).
            AppendLine("{n").
            AppendLine(Body?.ToString()).
            AppendLine("}").ToString();
    }
}