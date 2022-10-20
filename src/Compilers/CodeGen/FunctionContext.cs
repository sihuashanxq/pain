namespace Pain.Compilers.CodeGen;

public class FunctionContext
{
    public string Name { get; }

    public bool Static { get; }

    public object Type { get; }

    public FunctionFrame Frame { get; }

    public Dictionary<string, FunctionContext> Methods { get; }
}