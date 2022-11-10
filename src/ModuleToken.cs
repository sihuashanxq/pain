namespace Pain
{
    public class ModuleToken
    {
        public string Name { get; }

        public string Alias { get; }

        public string Module { get; }

        public ModuleToken(string module, string name, string alias = "")
        {
            Name = name;
            Alias = string.IsNullOrEmpty(alias) ? name : alias;
            Module = module;
        }

        public override string ToString()
        {
            return $"{Module}.{Name}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is ModuleToken token)
            {
                return token.Name == Name && token.Module == Module;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine<string, string>(Name, Module);
        }
    }
}