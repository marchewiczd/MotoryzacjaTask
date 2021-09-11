namespace Motoryzacja.Factories
{
    using System;
    
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    
    public static class WebDriverFactory
    {
        public static IWebDriver CreateDriver(WebDrivers driverType)
        {
            switch (driverType)
            {
                case WebDrivers.ChromeDriver:
                    return InitChromeDriver();
                default:
                    throw new NotImplementedException($"{driverType.ToString()} is not implemented.");
            }
        }

        private static ChromeDriver InitChromeDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument("--headless");
            
            ChromeDriver webDriver = new ChromeDriver(chromeOptions);
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            
            return webDriver;
        }
    }
}