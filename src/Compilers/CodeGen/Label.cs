
namespace Pain.Compilers.CodeGen;

public class Label
{
    public string Name { get; }

    public Operand<int> Target { get; }

    public Label(string name, Operand<int> target)
    {
        Name = name;
        Target = target;
    }
}