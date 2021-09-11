namespace Motoryzacja
{
    #region Usings

    using System;
    using System.IO;
    
    using Newtonsoft.Json;

    #endregion
    
    public class Config
    {
        #region Fields

        private static readonly Lazy<Config?> _instance = new Lazy<Config?>(() => 
            GetConfig());
        
        private const string FileName = "config.json"; 

        #endregion

        #region Constructor

        private Config() { }

        #endregion

        #region Properties

        public static Config Instance => _instance.Value;
        
        public string SearchPhrase { get; set; }
        
        public int NumberOfPagesRequested { get; set; }
        
        #endregion
        
        #region Methods

        private static Config GetConfig()
        {
            if (File.Exists(FileName))
            {
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(FileName));
            }
            
            string carMakeModel = String.Empty;
            string pageNumber = String.Empty;
            
            Console.WriteLine("Podaj marke i model samochodu: ");
            carMakeModel = Console.ReadLine();
            
            Console.WriteLine("Podaj ilosc stron: ");
            pageNumber = Console.ReadLine();

            return new Config() { SearchPhrase = carMakeModel, NumberOfPagesRequested = Int32.Parse(pageNumber) };
        }
        
        #endregion
    }
}