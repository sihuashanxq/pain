using System.IO;
namespace Pain
{
    public class FileReader
    {
        private static string _rootPath = "/Users/a000/Source/pain";

        public static string ReadFile(string path)
        {
            using (var sw = new System.IO.StreamReader(path))
            {
                return sw.ReadToEnd();
            }
        }

        public static string ReadModule(string token)
        {
            var path = string.Empty;
            path = token.Substring(0, token.LastIndexOf(".")).Replace(".", "/");
            return ReadFile(_rootPath + "/" + path + ".pp");
        }
    }
}