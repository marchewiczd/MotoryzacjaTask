using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Motoryzacja.Extensions;
using Newtonsoft.Json;

namespace Motoryzacja
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    
    class Program
    {
        static void Main(string[] args)
        {
            Config conf = Config.Instance;
            CarListPageObject carListPage = new CarListPageObject(GetChromeDriver());
            carListPage.GoToPage();
            carListPage.AcceptCookies();
            carListPage.InputSearchParams(conf.SearchPhrase);
            carListPage.PressSearchButton();
            List<CarData> carDataList = carListPage.GetCarsFromPages(conf.NumberOfPagesRequested).ToList();
            PrintAndSaveResults(carDataList, conf);
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
        
        private static IWebDriver GetChromeDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument("--headless");
            
            IWebDriver webDriver = new ChromeDriver(chromeOptions);
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //webDriver.Manage().Window.Maximize();
            
            
            return webDriver;
        }
        
    }
}