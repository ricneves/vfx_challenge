using AutoMapper;
using ExchangeRates.Application.ExchangeRates.Commands.CreateExchangeRate;
using ExchangeRates.Application.ExchangeRates.Commands.UpdateExchangeRate;
using ExchangeRates.Domain.Entities;

namespace ExchangeRates.Application.ExchangeRates.Dtos;

public class ExchangeRatesProfile : Profile
{
    public ExchangeRatesProfile()
    {
        CreateMap<ExchangeRate, ExchangeRateDto>();

        CreateMap<CreateExchangeRateCommand, ExchangeRate>();

        CreateMap<UpdateExchangeRateCommand, ExchangeRate>();

    }
}
