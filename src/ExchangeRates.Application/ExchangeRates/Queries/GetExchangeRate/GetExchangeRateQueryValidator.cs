using FluentValidation;

namespace ExchangeRates.Application.ExchangeRates.Queries.GetExchangeRate;

public class GetExchangeRateQueryValidator : AbstractValidator<GetExchangeRateQuery>
{
    public GetExchangeRateQueryValidator()
    {
        RuleFor(r => r.Date)
            .LessThanOrEqualTo(DateTime.UtcNow);

        RuleFor(r => r.FromCurrency)
            .Length(3);

        RuleFor(r => r.ToCurrency)
            .Length(3);
    }
}
