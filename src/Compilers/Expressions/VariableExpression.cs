namespace Pain.Compilers.Expressions;

public class VariableExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Var;

    public VaraibleDefinition[] Varaibles { get; }

    public VariableExpression(VaraibleDefinition[] varaibles)
    {
        Varaibles = varaibles;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitVariable(this);
    }
}

public class VaraibleDefinition
{
    public string Name { get; }

    public Syntax Value { get; }

    public VaraibleDefinition(string name, Syntax value)
    {
        Name = name;
        Value = value;
    }
}