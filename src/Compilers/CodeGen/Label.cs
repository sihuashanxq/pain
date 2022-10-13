
namespace Pain.Compilers.CodeGen;

public class Label
{
    public const string End = "end";

    public const string Next = "next";

    public const string True = "true";

    public const string False = "false";

    public const string Begin = "begin";

    public const string Break = "break";

    public const string Default = "default";

    public const string Continue = "continue";

    public string Name { get; }

    public Operand<int> Target { get; }

    public Label(string name, Operand<int> target)
    {
        Name = name;
        Target = target;
    }
}