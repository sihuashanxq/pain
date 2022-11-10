using System.IO;
namespace Pain
{
    public class ModuleReader
    {
        internal static string Runtime = Environment.CurrentDirectory;

        internal static string Project = Directory.GetCurrentDirectory();

        public static string Read(ModuleToken token)
        {
            var path = string.Empty;
            if (token.Module.StartsWith("@"))
            {
                path = $"{Runtime}/{token.Module.Substring(1).Replace(".", "/")}.pp";
            }
            else
            {
                path = $"{Project}/{token.Module.Replace(".", "/")}.pp";
            }

            using (var sw = new StreamReader(path))
            {
                return sw.ReadToEnd();
            }
        }
    }
}