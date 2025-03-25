using FluentValidation;

namespace ExchangeRates.Application.ExchangeRates.Commands.UpdateExchangeRate;

public class UpdateExchangeRateCommandValidator : AbstractValidator<UpdateExchangeRateCommand>
{
    public UpdateExchangeRateCommandValidator()
    {
        RuleFor(dto => dto.BidPrice)
            .GreaterThan(0);

        RuleFor(dto => dto.AskPrice)
            .GreaterThan(0);
    }
}
