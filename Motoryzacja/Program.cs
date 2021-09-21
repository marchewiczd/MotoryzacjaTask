using Motoryzacja.Providers;
using Motoryzacja.Validators;
using OpenQA.Selenium;

namespace Motoryzacja
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    
    using Newtonsoft.Json;
    
    using Configuration;
    using Data;
    using Extensions;
    using Factories;
    using PageObjects;
    
    class Program
    {
        static void Main(string[] args)
        {
            Config config = HandleConfigLoading();
            List<CarData> carDataList = RunWebsiteCrawl(config); 
            PrintAndSaveResults(carDataList, config);
        }

        private static void PrintAndSaveResults(List<CarData> carDataList, Config config)
        {
            double avgPrice = carDataList.CalculateAvgPrice();
            double avgMileage = carDataList.CalculateAvgMileage();
            double avgDisplacement = carDataList.CalculateAvgDisplacement();
            double avgYear = carDataList.CalculateAvgYear();
            
            Console.WriteLine($"Average price: {avgPrice}\n" +
                              $"Average mileage: {avgMileage}\n" +
                              $"Average displacement: {avgDisplacement}\n" +
                              $"Average year: {avgYear}");
            
            var avgCarData = new
            {
                SearchPhrase = config.SearchPhrase,
                NumberOfItems = carDataList.Count,
                AveragePrice = avgPrice,
                AverageMileage = avgMileage,
                AverageEngineCapacity = avgDisplacement,
                AverageYearOfProduction = avgYear
            };

            File.WriteAllText("results.json", JsonConvert.SerializeObject(avgCarData));
        }

        private static Config HandleConfigLoading()
        {
            ConfigFactory configFactory = new ConfigFactory();
            ConfigValidator validator = new ConfigValidator();
            
            Config conf = configFactory.LoadConfig<Config>();
            
            if (!validator.ValidateConfigObject(conf))
            {
                conf = configFactory.CreateConfigViaConsole(validator);
            }

            return conf;
        }

        private static List<CarData> RunWebsiteCrawl(Config config)
        {
            IWebDriver webDriver = WebDriverFactory.CreateDriver(DriverType.ChromeDriver);
            CarListPageObject carListPage = new CarListPageObject(webDriver);
            
            carListPage.GoToPage();
            carListPage.AcceptCookies();
            carListPage.InputSearchParams(config.SearchPhrase);
            carListPage.PressSearchButton();
            List<CarData> carDataList = carListPage.GetCarsFromPages(config.NumberOfPagesRequested).ToList();
            
            webDriver.Dispose();

            return carDataList;
        }
    }
}
