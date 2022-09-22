using System;
namespace Pain.Compilers.Parsers.Definitions
{
    public class ImportDefinition
    {
        public string Path { get; }

        public ImportClass[] Classes { get; }

        public ImportDefinition(string path, ImportClass[] classes)
        {
            Path = path;
            Classes = classes;
        }
    }

    public class ImportClass
    {
        public string Name { get; }

        public string Alias { get; }

        public ImportClass(string name,string alias)
        {
            Name = name;
            Alias = alias;
        }
    }
}