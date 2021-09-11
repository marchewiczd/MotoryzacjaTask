namespace Motoryzacja.PageObjects
{
    using System;
    
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    
    using System.Collections.Generic;
    
    using Extensions;
    using Data;
    
    public class CarListPageObject
    {
        private IWebDriver _webDriver;
        private const string PageUrl = "https://ogloszenia.trojmiasto.pl/motoryzacja-sprzedam/";
        private const string CookiesButtonXpath = "//button[@class=\" css-47sehv\"]";
        private const string CarMakeSelectXpath = "//select[@id=\"marka\"]";
        private const string CarModelSelectXpath = "//select[@id=\"model\"]";
        private const string SearchButtonXpath = "//button[@class=\"oglSearchbar__btn btn btn--orange\"]";
        private const string NextPageXpath = "//span[text()=\"następna\"]/parent::a";
        private const string CarListItemXpath = "//div[contains(@class, \"list--item--withPrice\")]";
        private const string CarPriceXpath = ".//p[@class=\"list__item__price__value\"]";
        private const string CarYearXpath = 
            ".//li[contains(@class, \"details--icons--element--rok_produkcji\")]/div[1]/p[2]";
        private const string CarMileageXpath = 
            ".//li[contains(@class, \"details--icons--element--przebieg\")]/div[1]/p[2]";
        private const string CarEngineCapacityXpath = 
            ".//li[contains(@class, \"details--icons--element--pojemnosc\")]/div[1]/p[2]";

        public CarListPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }
        
        public void GoToPage()
        {
            _webDriver.Navigate().GoToUrl(PageUrl);
        }
        
        public void AcceptCookies()
        {
            // workaround for cookies appearing twice
            try
            {
                _webDriver.FindElement(By.XPath(CookiesButtonXpath)).Click();
            }
            catch (StaleElementReferenceException)
            {
                _webDriver.FindElement(By.XPath(CookiesButtonXpath)).Click();
            }
        }

        public void InputSearchParams(string searchPhrase)
        {
            const int carMakeIndex = 0;
            const int carModelIndex = 1;

            string[] carInfo = searchPhrase.Split(" ");
            new SelectElement(_webDriver.FindElement(By.XPath(CarMakeSelectXpath))).SelectByText(carInfo[carMakeIndex]);
            new SelectElement(_webDriver.FindElement(By.XPath(CarModelSelectXpath))).SelectByText(carInfo[carModelIndex]);
        }

        public void PressSearchButton()
        {
            IWebElement searchButtonElement = _webDriver.FindElement(By.XPath(SearchButtonXpath));
            (_webDriver as IJavaScriptExecutor)
                .ExecuteScript("arguments[0].scrollIntoView(true);", searchButtonElement);
            searchButtonElement.Click();
        }

        public void GoToNextPage()
        {
            if (!NextPageExists())
            {
                return;
            }
            
            IWebElement nextPageElement = _webDriver.FindElement(By.XPath(NextPageXpath));
            (_webDriver as IJavaScriptExecutor)
                .ExecuteScript("arguments[0].scrollIntoView(true);", nextPageElement);
            nextPageElement.Click();
        }

        public IEnumerable<CarData> GetCarsFromPage()
        {
            var carElements = _webDriver.FindElements(By.XPath(CarListItemXpath));
            
            foreach (IWebElement carElement in carElements)
            {
                IWebElement priceElement = carElement.FindElementIfExists(By.XPath(CarPriceXpath));
                
                yield return new CarData()
                {
                    Price = Int32.Parse(priceElement.Text
                                            .Replace("zł", string.Empty)
                                            .Replace(" ", string.Empty)),
                    ProductionYear = carElement.FindElementIfExists(By.XPath(CarYearXpath)).GetTextAsInt(),
                    Mileage = carElement.FindElementIfExists(By.XPath(CarMileageXpath)).GetTextAsInt(),
                    Displacement = carElement.FindElementIfExists(By.XPath(CarEngineCapacityXpath)).GetTextAsInt()
                };
            }
        }

        public IEnumerable<CarData> GetCarsFromPages(int numberOfPages = 0)
        {
            List<CarData> results = new List<CarData>();
            
            if (numberOfPages == 0)
            {
                results.AddRange(GetCarsFromPage());
                
                while(NextPageExists())
                {
                    GoToNextPage();
                    results.AddRange(GetCarsFromPage());
                }

                return results;
            }

            for (int i = 0; i < numberOfPages; i++)
            {
                results.AddRange(GetCarsFromPage());

                if (!NextPageExists())
                {
                    return results;
                }
                
                GoToNextPage();
            }
            
            return results;
        }

        public bool NextPageExists()
        {
            try
            {
                _webDriver.FindElement(By.XPath(NextPageXpath));
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }
    }

}