using System;
using FluentAssertions;
using Motoryzacja.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Motoryzacja.Tests
{
    public class WebElementExtensionsTests
    {
        private IWebElement _webElement = Substitute.For<IWebElement>();
        
        [Fact]
        public void FindElementIfExists_ShouldReturnWebElement_WhenElementExists()
        {
            _webElement.FindElement(null).Returns(new ChromeWebElement(null, "id"));

            IWebElement actualResult = _webElement.FindElementIfExists(null);

            actualResult.Should().NotBeNull();
            actualResult.Should().BeOfType(typeof(ChromeWebElement));
        }
        
        [Fact]
        public void FindElementIfExists_ShouldReturnNull_WhenElementDoesNotExist()
        {
            _webElement.FindElement(null).Throws(new NoSuchElementException());

            IWebElement actualResult = _webElement.FindElementIfExists(null);

            actualResult.Should().BeNull();
        } 
        
        [Fact]
        public void GetTextAsInt_ShouldReturnZero_WhenWebElementIsNull()
        {
            IWebElement actualResult = null;
            actualResult.GetTextAsInt().Should().Be(0);
        } 
        
        [Fact]
        public void GetTextAsInt_ShouldReturnZero_WhenTextFormatIsIncorrect()
        {
            _webElement.Text.Returns("abc");
            _webElement.GetTextAsInt().Should().Be(0);
        }
        
        [Fact]
        public void GetTextAsInt_ShouldReturnInteger_WhenInputIsValid()
        {
            int expectedInteger = 523;
            
            _webElement.Text.Returns(expectedInteger.ToString());
            _webElement.GetTextAsInt().Should().Be(expectedInteger);
        } 
    }
}