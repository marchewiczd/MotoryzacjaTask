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
            Config conf = Config.Instance;
            IWebDriver webDriver = WebDriverFactory.CreateDriver(WebDrivers.ChromeDriver);
            CarListPageObject carListPage = new CarListPageObject(webDriver);
            
            carListPage.GoToPage();
            carListPage.AcceptCookies();
            carListPage.InputSearchParams(conf.SearchPhrase);
            carListPage.PressSearchButton();
            List<CarData> carDataList = carListPage.GetCarsFromPages(conf.NumberOfPagesRequested).ToList();
            PrintAndSaveResults(carDataList, conf);

            webDriver.Dispose();
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
        
    }
}