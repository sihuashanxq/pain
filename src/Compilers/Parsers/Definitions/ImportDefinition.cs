using System;
namespace Pain.Compilers.Parsers.Definitions
{
    public class ImportDefinition
    {
        public string Name { get; }

        public string Alias { get; }

        public string Module { get; }

        public string Token { get; }

        public ImportDefinition(string name, string alias, string module)
        {
            Name = name;
            Alias = alias;
            Token=$"{module}.{name}";
            Module = module;
        }
    }
}