namespace Pain.Compilers.Expressions;

public class CallExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Call;

    public Syntax Function { get; }

    public Syntax[] Arguments { get; }

    public CallExpression(Syntax function, Syntax[] arguments)
    {
        Function = function;
        Arguments = arguments;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitCall(this);
    }

    public override string ToString()
    {
        return $"{Function}({string.Join(",", Arguments.Select(i => i.ToString()))})";
    }
}