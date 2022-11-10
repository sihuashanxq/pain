namespace Pain.Compilers.CodeGen;

public class Variable
{
    public int Slot { get; }

    public string Name { get; }

    public Variable(string name, int slot)
    {
        Name = name;
        Slot = slot;
    }
}