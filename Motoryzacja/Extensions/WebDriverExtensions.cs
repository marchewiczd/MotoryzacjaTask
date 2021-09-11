namespace Motoryzacja.Extensions
{
    using OpenQA.Selenium;

    public static class WebDriverExtensions
    {
        public static IWebElement FindElementIfExists(this IWebDriver webDriver, By by)
        {
            try
            {
                return webDriver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }
    }
}