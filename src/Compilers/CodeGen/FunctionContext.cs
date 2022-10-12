namespace Pain.Compilers.CodeGen;

public class FunctionContext
{
    public int Token { get; }

    public string Name { get; }

    public bool Static { get; }

    public TypeContext Type { get; }

    public FunctionFrame Frame { get; }

    public Dictionary<string, FunctionContext> LocalMethods { get; }
}