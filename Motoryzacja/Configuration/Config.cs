namespace Motoryzacja.Configuration
{
    using System;
    using System.IO;
    
    using Newtonsoft.Json;
    
    public class Config
    {
        private static readonly Lazy<Config?> _instance = new Lazy<Config?>(() => 
            LoadConfigFromFile() ?? CreateConfigViaConsole());
        
        private const string FileName = "config.json"; 

        private Config() { }

        public static Config Instance => _instance.Value;
        
        public string SearchPhrase { get; private init; }
        
        public int NumberOfPagesRequested { get; private init; }

        private static Config LoadConfigFromFile()
        {
            if (File.Exists(FileName))
            {
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(FileName));
            }

            return null;
        }

        private static Config CreateConfigViaConsole()
        {
            Console.WriteLine("Podaj marke i model samochodu: ");
            string carMakeModel = Console.ReadLine();
            
            Console.WriteLine("Podaj ilosc stron: ");
            string pageNumber = Console.ReadLine();

            return new Config() { SearchPhrase = carMakeModel, NumberOfPagesRequested = Int32.Parse(pageNumber) };
        }
    }
}