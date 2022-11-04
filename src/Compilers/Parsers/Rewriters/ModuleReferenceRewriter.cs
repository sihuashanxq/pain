using Pain.Compilers.Expressions;
using Pain.Compilers.Parsers.Definitions;
namespace Pain.Compilers.Parsers.Rewriters;

public class ModuleReferenceRewriter
{
    private readonly ClassDefinition _class;

    private readonly ModuleDefinition _module;

    public ModuleReferenceRewriter(ModuleDefinition module, ClassDefinition @class)
    {
        _class = @class;
        _module = module;
    }
}