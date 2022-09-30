namespace Pain.Compilers.CodeGen;

public class Variable
{
    public int Slot { get; }

    public string Name { get; }

    public bool Captured { get;  set; }

    public Variable CapturedVariable { get;  set; }

    public Variable(string name, int slot, bool captured, Variable capturedVariable)
    {
        Name = name;
        Slot = slot;
        Captured = captured;
        CapturedVariable = capturedVariable;
    }

    public Variable Capture()
    {
        return new Variable(Name, Slot, true, this);
    }
}