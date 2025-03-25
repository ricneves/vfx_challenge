using AutoMapper;
using ExchangeRates.Application.ExchangeRates.Commands.CreateExchangeRate;
using ExchangeRates.Application.ExchangeRates.Commands.UpdateExchangeRate;
using ExchangeRates.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ExchangeRates.Application.ExchangeRates.Dtos.Tests;

public class ExchangeRatesProfileTests
{
    private IMapper _mapper;

    public ExchangeRatesProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ExchangeRatesProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForExchangeRateToExchangeRateDto_MapsCorrectly()
    {
        // arrange
        var exchangeRate = new ExchangeRate()
        {
            Id = 1,
            FromCurrency = "EUR",
            ToCurrency = "USD",
            Date = DateTime.UtcNow,
            AskPrice = 1.01m,
            BidPrice = 1.03m
        };

        // act
        var exchangeRateDto = _mapper.Map<ExchangeRateDto>(exchangeRate);

        // assert
        exchangeRateDto.Should().NotBeNull();
        exchangeRateDto.FromCurrency.Should().Be(exchangeRate.FromCurrency);
        exchangeRateDto.ToCurrency.Should().Be(exchangeRate.ToCurrency);
        exchangeRateDto.Date.Should().Be(exchangeRate.Date);
        exchangeRateDto.AskPrice.Should().Be(exchangeRate.AskPrice);
        exchangeRateDto.BidPrice.Should().Be(exchangeRate.BidPrice);
    }

    [Fact()]
    public void CreateMap_ForCreateExchangeRateCommandToexchangeRate_MapsCorrectly()
    {
        // arrange
        var command = new CreateExchangeRateCommand()
        {
            FromCurrency = "EUR",
            ToCurrency = "USD",
            Date = DateTime.UtcNow,
            AskPrice = 1.01m,
            BidPrice = 1.03m
        };

        // act
        var exchangeRate = _mapper.Map<ExchangeRate>(command);

        // assert
        exchangeRate.Should().NotBeNull();
        exchangeRate.FromCurrency.Should().Be(command.FromCurrency);
        exchangeRate.ToCurrency.Should().Be(command.ToCurrency);
        exchangeRate.Date.Should().Be(command.Date);
        exchangeRate.AskPrice.Should().Be(command.AskPrice);
        exchangeRate.BidPrice.Should().Be(command.BidPrice);
    }

    [Fact()]
    public void CreateMap_ForUpdateExchangeRateCommandToExchangeRate_MapsCorrectly()
    {
        // arrange
        var command = new UpdateExchangeRateCommand()
        {
            Id = 1,
            AskPrice = 1.01m,
            BidPrice = 1.03m
        };

        // act
        var exchangeRate = _mapper.Map<ExchangeRate>(command);

        // assert
        exchangeRate.Should().NotBeNull();
        exchangeRate.Id.Should().Be(command.Id);
        exchangeRate.AskPrice.Should().Be(command.AskPrice);
        exchangeRate.BidPrice.Should().Be(command.BidPrice);
    }
}