namespace Motoryzacja.Factories
{
    using System;
    
    using Newtonsoft.Json;
    
    using Configuration;
    using Validators;
    using Providers;
    
    public class ConfigFactory
    {
        private IFileProvider _fileProvider;
        private IConsoleInputProvider _consoleInputProvider;
        private string _filePath;
        
        public ConfigFactory(string filePath, IFileProvider fileProvider, IConsoleInputProvider consoleInputProvider)
        {
            _filePath = filePath;
            _fileProvider = fileProvider;
            _consoleInputProvider = consoleInputProvider;
        }

        public ConfigFactory() :
            this(
                "config.json",
                new FileProvider(),
                new ConsoleInputProvider())
        {
        }
        
        public T LoadConfig<T>()
        {
            string fileString = _fileProvider.ReadAllTextFromFile(_filePath);
            
            return JsonConvert.DeserializeObject<T>(fileString);;
        }


        public Config CreateConfigViaConsole(ConfigValidator configValidator)
        {
            string carMakeModel, pageCount;

            do
            {
                Console.WriteLine("Podaj marke i model samochodu: ");
                carMakeModel = _consoleInputProvider.ReadLine();
            } while (!configValidator.ValidateCarInput(carMakeModel));

            do
            {
                Console.WriteLine("Podaj ilosc stron: ");
                pageCount = _consoleInputProvider.ReadLine();
            } while (!configValidator.ValidatePageInput(pageCount));

            return new Config() { SearchPhrase = carMakeModel, NumberOfPagesRequested = Int32.Parse(pageCount) };
        }
    }
}