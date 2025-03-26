﻿using ExchangeRates.Domain.Entities;
using FluentValidation;

namespace ExchangeRates.Application.ExchangeRates.Queries.GellAllExchangeRates;

public class GetAllExchangeRatesQueryValidator : AbstractValidator<GetAllExchangeRatesQuery>
{
    private int[] allowPageSizes = [5, 10, 15, 30];

    private string[] allowedSortByColumnNames = [nameof(ExchangeRate.FromCurrency), nameof(ExchangeRate.ToCurrency), nameof(ExchangeRate.Date)];

    public GetAllExchangeRatesQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowPageSizes.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(",", allowPageSizes)}].");

        RuleFor(r => r.SortBy)
            .Must(value => allowedSortByColumnNames.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}].");
    }
}
