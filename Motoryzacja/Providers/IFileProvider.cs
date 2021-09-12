using System.IO;

namespace Motoryzacja.Providers
{
    public interface IFileProvider
    {
        public string ReadAllTextFromFile(string filePath);
    }
}