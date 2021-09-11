namespace Motoryzacja.Extensions
{
    using System;
    using OpenQA.Selenium;
    
    public static class WebElementExtensions
    {
        public static IWebElement FindElementIfExists(this IWebElement webElement, By by)
        {
            try
            {
                return webElement.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public static int GetTextAsInt(this IWebElement webElement)
        {
            try
            {
                return webElement != null ? Int32.Parse(webElement.Text) : 0;
            }
            catch (FormatException)
            {
                return 0;
            }
        }
    }
}