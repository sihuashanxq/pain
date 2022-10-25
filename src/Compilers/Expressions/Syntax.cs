namespace Pain.Compilers.Expressions;

public abstract class Syntax
{
    public abstract SyntaxType Type { get; }

    public abstract T Accept<T>(SyntaxVisitor<T> visitor);

    public static ParameterExpression Parameter(string name, int index)
    {
        return new ParameterExpression(name, index);
    }

    public static ThisExpression This()
    {
        return new ThisExpression();
    }

    public static SuperExpression Super()
    {
        return new SuperExpression();
    }

    public static NewExpression New(Syntax @class, Syntax[] arguments)
    {
        return new NewExpression(@class, arguments);
    }

    public static ReturnExpression Return(Syntax value)
    {
        return new ReturnExpression(value);
    }

    public static UnaryExpression Unary(Syntax expr, SyntaxType type)
    {
        return new UnaryExpression(expr, type);
    }

    public static MemberExpression Member(Syntax @object, Syntax member)
    {
        return new MemberExpression(@object, member);
    }

    public static NameExpression Name(string name)
    {
        return new NameExpression(name);
    }

    public static BinaryExpression Binary(Syntax left, Syntax right, SyntaxType type)
    {
        return new BinaryExpression(left, right, type);
    }

    public static BlockExpression Block(Syntax[] statements)
    {
        return new BlockExpression(statements);
    }

    public static ConstantExpression Constant(object value, SyntaxType type)
    {
        return new ConstantExpression(value, type);
    }

    public static CallExpression Call(Syntax function, Syntax[] arguments)
    {
        return new CallExpression(function, arguments);
    }

    public static ContinueExpression Continue()
    {
        return new ContinueExpression();
    }

    public static BreakExpression Break()
    {
        return new BreakExpression();
    }
}