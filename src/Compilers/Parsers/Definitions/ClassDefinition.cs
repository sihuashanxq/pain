using Pain.Compilers.Expressions;
namespace Pain.Compilers.Parsers.Definitions
{
    public class ClassDefinition
    {
        public string Name { get; }

        public string Super { get; }

        public Syntax[] Functions { get; }

        public ClassDefinition(string name,string super, Syntax[] functions)
        {
            Name = name;
            Super = super;
            Functions = functions;
        }
    }
}

