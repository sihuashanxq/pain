namespace Pain.Compilers.Expressions;

public abstract class Syntax
{
    public abstract SyntaxType Type { get; }

    public abstract T Accept<T>(SyntaxVisitor<T> visitor);
}