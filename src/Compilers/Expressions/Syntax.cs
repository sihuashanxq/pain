namespace Pain.Compilers.Expressions;

public abstract class Syntax
{
    public abstract SyntaxType Type { get; }

    public abstract T Accept<T>(SyntaxVisitor<T> visitor);

    public static ParameterExpression MakeParameter(string name, int index)
    {
        return new ParameterExpression(name, index);
    }

    public static ThisExpression MakeThis()
    {
        return new ThisExpression();
    }

    public static SuperExpression MakeSuper()
    {
        return new SuperExpression();
    }

    public static NewExpression MakeNew(Syntax @class, Syntax[] arguments)
    {
        return new NewExpression(@class, arguments);
    }

    public static ReturnExpression MakeReturn(Syntax value)
    {
        return new ReturnExpression(value);
    }

    public static UnaryExpression MakeUnary(Syntax expr, SyntaxType type)
    {
        return new UnaryExpression(expr, type);
    }

    public static MemberExpression MakeMember(Syntax @object, Syntax member)
    {
        return new MemberExpression(@object, member);
    }

    public static NameExpression MakeName(string name)
    {
        return new NameExpression(name);
    }

    public static BinaryExpression MakeBinary(Syntax left, Syntax right, SyntaxType type)
    {
        return new BinaryExpression(left, right, type);
    }

    public static BlockExpression MakeBlock(params Syntax[] statements)
    {
        return new BlockExpression(statements);
    }

    public static ConstantExpression MakeConstant(object value, SyntaxType type)
    {
        return new ConstantExpression(value, type);
    }

    public static CallExpression MakeCall(Syntax function, Syntax[] arguments)
    {
        return new CallExpression(function, arguments);
    }

    public static BreakExpression MakeBreak()
    {
        return new BreakExpression();
    }

    public static ContinueExpression MakeContinue()
    {
        return new ContinueExpression();
    }

    public static MemberInitExpression MakeMemberInit(Syntax @new, Dictionary<string, Syntax> members)
    {
        return new MemberInitExpression(@new, members);
    }

    public static FunctionExpression MakeFunction(string name, bool isLocal, ParameterExpression[] parameters, Syntax body)
    {
        return new FunctionExpression(name, isLocal, parameters, body);
    }

    public static IfExpression MakeIf(Syntax test, Syntax ifTrue, Syntax ifFalse)
    {
        return new IfExpression(test, ifTrue, ifFalse);
    }

    public static EmptyExpression MakeEmpty()
    {
        return new EmptyExpression();
    }

    public static VariableExpression MakeVariable(Varaible[] varaibles)
    {
        return new VariableExpression(varaibles);
    }

    public static ForExpression MakeFor(Syntax[] initializers, Syntax test, Syntax[] iterators, Syntax body)
    {
        return new ForExpression(initializers, test, iterators, body);
    }

    public static ArrayInitExpression MakeArray(Syntax[] items)
    {
        return new ArrayInitExpression(items);
    }
}