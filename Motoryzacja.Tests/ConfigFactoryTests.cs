
namespace Motoryzacja.Tests
{
    using FluentAssertions;
    
    using Motoryzacja.Configuration;
    using Motoryzacja.Factories;
    using Motoryzacja.Providers;
    using Motoryzacja.Validators;
    
    using NSubstitute;
    
    using Xunit;

    
    public class ConfigFactoryTests
    {
        private ConfigFactory _testedObject;
        private readonly IFileProvider _fileProvider = Substitute.For<IFileProvider>();
        private readonly IConsoleInputProvider _consoleInputProvider = Substitute.For<IConsoleInputProvider>();
        private readonly string _filePath = "config.json";
        
        public ConfigFactoryTests()
        {
            _testedObject = new ConfigFactory(_filePath, _fileProvider, _consoleInputProvider);
        }
        
        [Fact]
        public void LoadConfig_ShouldReturnConfig_WhenProvidedWithValidData()
        {
            const string SearchPhrase = "BMW E200";
            const int NumberOfPages = 3;
            string ValidJsonConfig = 
                "{\"SearchPhrase\":\"" + SearchPhrase + "\",\"NumberOfPagesRequested\":" + NumberOfPages + "}";
            
            _fileProvider.ReadAllTextFromFile(_filePath).Returns(ValidJsonConfig);
            Config loadedConfig = _testedObject.LoadConfig<Config>();
            
            loadedConfig.SearchPhrase.Should().Be(SearchPhrase);
            loadedConfig.NumberOfPagesRequested.Should().Be(NumberOfPages);
        }
        
        [Fact]
        public void LoadConfig_ShouldReturnNull_WhenJsonConfigInvalid()
        {
            const string SearchPhrase = "BMW E200";
            const int NumberOfPages = 3;
            string ValidJsonConfig = 
                "{\"BadJson\": \"Asd7231\"}";
            
            _fileProvider.ReadAllTextFromFile(_filePath).Returns(ValidJsonConfig);
            Config loadedConfig = _testedObject.LoadConfig<Config>();

            loadedConfig.SearchPhrase.Should().Be(default);
            loadedConfig.NumberOfPagesRequested.Should().Be(default);
        }
        
        [Fact]
        public void LoadConfig_ShouldCreateConfig_WhenConfigMissingNumberOfPages()
        {
            const string SearchPhrase = "Audi A3";
            const int ExpectedPageCount = 0;
            string ValidJsonConfig = "{\"SearchPhrase\":\"" + SearchPhrase + "\"}";
            
            _fileProvider.ReadAllTextFromFile(_filePath).Returns(ValidJsonConfig);
            
            Config loadedConfig = _testedObject.LoadConfig<Config>();

            loadedConfig.SearchPhrase.Should().Be(SearchPhrase);
            loadedConfig.NumberOfPagesRequested.Should().Be(ExpectedPageCount);
        }
        
        [Fact]
        public void LoadConfig_ShouldReturnNull_WhenFileDoesNotExist()
        {
            _fileProvider.ReadAllTextFromFile(_filePath).Returns(string.Empty);
            
            Config loadedConfig = _testedObject.LoadConfig<Config>();

            loadedConfig.Should().BeNull();
        }

        [Fact]
        public void CreateConfigViaConsole_ShouldCreateConfig_WhenInputValid()
        {
            const int NumberOfPages = 3;
            const string SearchPhrase = "Opel Astra";
            ConfigValidator validator = new ConfigValidator();
            _consoleInputProvider.ReadLine().Returns(SearchPhrase, NumberOfPages.ToString());
            
            Config loadedConfig = _testedObject.CreateConfigViaConsole(validator);

            loadedConfig.SearchPhrase.Should().Be(SearchPhrase);
            loadedConfig.NumberOfPagesRequested.Should().Be(NumberOfPages);
        }

        [Fact]
        public void CreateConfigViaConsole_ShouldKeepAskingForInput_WhenInputIsNotValid()
        {
            const int NumberOfPages = 123;
            const int NumberOfPagesBadInput = -5;
            const string SearchPhrase = "Xyz Asdef";
            const string SearchPhraseBadInput = "Bad Input For Search Phrase";
            ConfigValidator validator = new ConfigValidator();
            _consoleInputProvider.ReadLine().Returns(SearchPhraseBadInput, SearchPhrase, NumberOfPagesBadInput.ToString(), NumberOfPages.ToString());
            
            Config loadedConfig = _testedObject.CreateConfigViaConsole(validator);

            loadedConfig.SearchPhrase.Should().Be(SearchPhrase);
            loadedConfig.NumberOfPagesRequested.Should().Be(NumberOfPages);
        }
    }
}