using Motoryzacja.Validators;

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
        
        public string SearchPhrase { get; init; }
        
        public int NumberOfPagesRequested { get; init; }

        private static Config LoadConfigFromFile()
        {
            if (File.Exists(FileName))
            {
                Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(FileName));
                ConfigValidator configValidator = new ConfigValidator();

                if (config != null
                    && configValidator.ValidateCarInput(config.SearchPhrase)
                    && configValidator.ValidatePageInput(config.NumberOfPagesRequested))
                {
                    return config;
                }

                throw new ArgumentException($"Wartosci podane w konfiguracji sa niepoprawne:" +
                                        $"\n\tSearchPhrase: {config.SearchPhrase}" +
                                        $"\n\tNumberOfPagesRequested: {config.NumberOfPagesRequested}");
            }

            return null;
        }

        private static Config CreateConfigViaConsole()
        {
            ConfigValidator configValidator = new ConfigValidator();
            string carMakeModel = string.Empty;
            string pageNumber = string.Empty;

            do
            {
                Console.WriteLine("Podaj marke i model samochodu: ");
                carMakeModel = Console.ReadLine();
            } while (!configValidator.ValidateCarInput(carMakeModel));
            
            do
            {
                Console.WriteLine("Podaj ilosc stron: ");
                pageNumber = Console.ReadLine();
            } while (!configValidator.ValidatePageInput(pageNumber));


            return new Config() { SearchPhrase = carMakeModel, NumberOfPagesRequested = Int32.Parse(pageNumber) };
        }
    }
}