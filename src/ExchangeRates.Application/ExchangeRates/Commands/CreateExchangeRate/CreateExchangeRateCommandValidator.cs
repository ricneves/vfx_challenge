using FluentValidation;

namespace ExchangeRates.Application.ExchangeRates.Commands.CreateExchangeRate;

public class CreateExchangeRateCommandValidator : AbstractValidator<CreateExchangeRateCommand>
{
    public CreateExchangeRateCommandValidator()
    {
        RuleFor(dto => dto.FromCurrency)
            .Length(3);

        RuleFor(dto => dto.ToCurrency)
            .Length(3);

        RuleFor(dto => dto.BidPrice)
            .GreaterThan(0);

        RuleFor(dto => dto.AskPrice)
            .GreaterThan(0);

        RuleFor(r => r.Date)
            .LessThanOrEqualTo(DateTime.UtcNow);
    }
}
