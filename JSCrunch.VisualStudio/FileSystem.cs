using System.IO;

namespace JSCrunch.VisualStudio
{
    public class FileSystem : IFileSystem
    {
        public string GetContentsOf(string path)
        {
            return File.ReadAllText(path);
        }
    }
}