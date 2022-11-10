namespace Pain.Compilers.Expressions;

public class VariableExpression : Syntax
{
    public override SyntaxType Type => SyntaxType.Let;

    public Variable[] Varaibles { get; }

    public VariableExpression(Variable[] varaibles)
    {
        Varaibles = varaibles;
    }

    public override T Accept<T>(SyntaxVisitor<T> visitor)
    {
        return visitor.VisitVariable(this);
    }

    public override string ToString()
    {
        return $"let {string.Join(",", Varaibles.Select(i => i.ToString()))}";
    }
}

public class Variable : ICaptureable
{
    public string Name { get; }

    public Syntax Value { get; }

    public Variable(string name, Syntax value)
    {
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return Value == null ? $"{Name}" : $"{Name}={Value}";
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is ICaptureable name)
        {
            return name.Name == this.Name;
        }

        return false;
    }
}