namespace Motoryzacja.Providers
{
    using System.IO;
    
    public class FileProvider : IFileProvider
    {
        public string ReadAllTextFromFile(string filePath)
        {
            return File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;
        }
    }
}