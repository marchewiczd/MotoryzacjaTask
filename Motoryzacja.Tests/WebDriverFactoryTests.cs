using FluentAssertions;
using Motoryzacja.Factories;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Motoryzacja.Tests
{
    public class WebDriverFactoryTests
    {
        [Fact]
        public void CreateDriver_ShouldReturnChromeDriver_WhenGivenChromeInput()
        {
            IWebDriver webDriver = WebDriverFactory.CreateDriver(DriverType.ChromeDriver);
            webDriver.Should().BeOfType(typeof(ChromeDriver));
            webDriver.Dispose();
        }
    }
}