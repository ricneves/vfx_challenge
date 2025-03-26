using ExchangeRates.Domain.Constants;
using ExchangeRates.Domain.Entities;

namespace ExchangeRates.Domain.Interfaces;

public interface IApplicationAuthorizationService
{
    bool Authorize(ExchangeRate exchangeRate, ResourceOperation resourceOperation);
}