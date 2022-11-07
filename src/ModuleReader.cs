using System.IO;
namespace Pain
{
    public class ModuleReader
    {
        internal static string Runtime = Environment.CurrentDirectory;

        internal static string Project = Directory.GetCurrentDirectory();

        public static string Read(string token)
        {
            var path = string.Empty;
            if (token.StartsWith("@"))
            {
                path = $"{Runtime}/{token.Substring(0, token.LastIndexOf(".")).Replace(".", "/")}.pp";
            }
            else
            {
                path = $"{Project}/{token.Substring(0, token.LastIndexOf(".")).Replace(".", "/")}.pp";
            }

            using (var sw = new StreamReader(path))
            {
                return sw.ReadToEnd();
            }
        }
    }
}