using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Motoryzacja.Configuration;
using Motoryzacja.Validators;
using Xunit;

namespace Motoryzacja.Tests
{
    public class ConfigValidatorTests
    {
        private readonly ConfigValidator _configValidator = new ConfigValidator();
        
        [Theory]
        [InlineData("Audi A3")]
        [InlineData("AbAb 123")]
        [InlineData("123 abed")]
        [InlineData("AAACCC 12345")]
        [InlineData("A12Acc54CC 1a23bbx5")]
        public void ValidateCarInput_ShouldBeTrue_WhenInputValid(string input)
        {
            bool actualResult = _configValidator.ValidateCarInput(input);
            actualResult.Should().BeTrue();
        }
        
        [Theory]
        [InlineData("Audi ą3")]
        [InlineData(";;; asdfi")]
        [InlineData("123.123 AUD0")]
        public void ValidateCarInput_ShouldBeTrue_WhenInputHasIncorrectChars(string input)
        {
            bool actualResult = _configValidator.ValidateCarInput(input);
            actualResult.Should().BeFalse();
        }
        
        [Theory]
        [InlineData("Audi A3 B5")]
        [InlineData("A b c")]
        [InlineData("1 2 3")]
        [InlineData("a b 1 2")]
        public void ValidateCarInput_ShouldBeTrue_WhenInputIsMoreThan2Words(string input)
        {
            bool actualResult = _configValidator.ValidateCarInput(input);
            actualResult.Should().BeFalse();
        }
        
        [Fact]
        public void ValidateCarInput_ShouldBeTrue_WhenInputIsEmpty()
        {
            bool actualResult = _configValidator.ValidateCarInput(string.Empty);
            actualResult.Should().BeFalse();
        }
        
        [Fact]
        public void ValidateCarInput_ShouldBeTrue_WhenInputIsNull()
        {
            bool actualResult = _configValidator.ValidateCarInput(null);
            actualResult.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(1358)]
        public void ValidatePageInput_ShouldBeTrue_WhenIntegerInputIsValid(int input)
        {
            bool actualResult = _configValidator.ValidatePageInput(input);
            actualResult.Should().BeTrue();
        }
        
        [Fact]
        public void ValidatePageInput_ShouldBeFalse_WhenIntegerInputIsNegative()
        {
            bool actualResult = _configValidator.ValidatePageInput(-5);
            actualResult.Should().BeFalse();
        }
        
        [Theory]
        [InlineData("0")]
        [InlineData("7")]
        [InlineData("1248")]
        public void ValidatePageInput_ShouldBeTrue_WhenStringInputIsValid(string input)
        {
            bool actualResult = _configValidator.ValidatePageInput(input);
            actualResult.Should().BeTrue();
        }
        
        [Fact]
        public void ValidatePageInput_ShouldBeFalse_WhenStringInputIsNegative()
        {
            bool actualResult = _configValidator.ValidatePageInput("-5");
            actualResult.Should().BeFalse();
        }
        
        [Fact]
        public void ValidatePageInput_ShouldBeFalse_WhenStringInputIsEmpty()
        {
            bool actualResult = _configValidator.ValidatePageInput(string.Empty);
            actualResult.Should().BeFalse();
        }
        
        [Fact]
        public void ValidateConfigObject_ShouldBeTrue_WhenInputValid()
        {
            Config config = new Config() { NumberOfPagesRequested = 5, SearchPhrase = "Opel Astra" };
            bool actualResult = _configValidator.ValidateConfigObject(config);
            actualResult.Should().BeTrue();
        }
        
        [Fact]
        public void ValidateConfigObject_ShouldBeFalse_WhenConfigIsNull()
        {
            bool actualResult = _configValidator.ValidateConfigObject(null);
            actualResult.Should().BeFalse();
        }
    }
}