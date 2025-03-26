using ExchangeRates.Application.ExchangeRates.Dtos;
using MediatR;

namespace ExchangeRates.Application.ExchangeRates.Queries.GetExchangeRateById;

public class GetExchangeRateByIdQuery(int id) : IRequest<ExchangeRateDto>
{
    public int Id { get; } = id;
}
