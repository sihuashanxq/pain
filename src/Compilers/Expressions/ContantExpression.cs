namespace Pain.Compilers.Expressions;

public class ConstantExpression : Syntax
{
    public override SyntaxType Type { get; }

    public object Value { get; }

    public ConstantExpression(object value,SyntaxType type)
    {
        Type = type;
        Value = value;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitConstant(this);
    }

    public override string ToString()
    {
        return Value?.ToString()??string.Empty;
    }
}