using System;
namespace Pain.Compilers.Parsers.Definitions
{
    public class Import
    {
        public string Name { get; }

        public string Alias { get; }

        public string Module { get; }

        public Import(string name, string alias, string module)
        {
            Name = name;
            Alias = alias;
            Module = module;
        }
    }
}