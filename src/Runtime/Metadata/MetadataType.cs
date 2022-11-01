namespace Pain.Runtime.Metadata;

public class MetadataType
{
    public string Name { get; }
    
    public string Module { get; }

    public MetadataType Super { get; }

    public FunctionTable FunctionTable { get; }

    public MetadataType(MetadataType super, string name, string module, FunctionTable functionTable)
    {
        Name = name;
        Super = super;
        Module = module;
        FunctionTable = functionTable;
    }
}