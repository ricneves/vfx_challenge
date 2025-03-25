using FluentValidation.TestHelper;
using Xunit;

namespace ExchangeRates.Application.ExchangeRates.Commands.CreateExchangeRate.Tests;

public class CreateExchangeRateCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
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

        var validator = new CreateExchangeRateCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // arrange
        var command = new CreateExchangeRateCommand()
        {
            FromCurrency = "EURR",
            ToCurrency = "US",
            Date = DateTime.UtcNow.AddDays(1),
            AskPrice = -1.01m,
            BidPrice = -1.03m
        };

        var validator = new CreateExchangeRateCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveValidationErrorFor(c => c.FromCurrency);
        result.ShouldHaveValidationErrorFor(c => c.ToCurrency);
        result.ShouldHaveValidationErrorFor(c => c.AskPrice);
        result.ShouldHaveValidationErrorFor(c => c.BidPrice);
        result.ShouldHaveValidationErrorFor(c => c.Date);
    }
}