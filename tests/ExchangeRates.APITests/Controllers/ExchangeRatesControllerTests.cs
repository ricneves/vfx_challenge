using ExchangeRates.APITests;
using ExchangeRates.Application.ExchangeRates.Dtos;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Repositories;
using ExchangeRates.Infrastructure.Seeders;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ExchangeRates.API.Controllers.Tests
{
    public class ExchangeRatesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IExchangeRatesRepository> _exchangeRatesRepositoryMock = new();
        private readonly Mock<IApplicationSeeder> _applicationSeederMock = new();

        public ExchangeRatesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.Replace(ServiceDescriptor.Scoped(typeof(IExchangeRatesRepository), _ => _exchangeRatesRepositoryMock.Object));
                    services.Replace(ServiceDescriptor.Scoped(typeof(IApplicationSeeder), _ => _applicationSeederMock.Object));
                });
            });
        }

        [Fact()]
        public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
        {
            // arrange
            var id = 1234;

            _exchangeRatesRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((ExchangeRate?)null);

            var client = _factory.CreateClient();

            // act
            var result = await client.GetAsync($"/api/ExchangeRates/{id}");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact()]
        public async Task GetById_ForExistingId_ShouldReturn200Ok()
        {
            // arrange
            var id = 99;

            var exchangeRate = new ExchangeRate
            {
                Id = id,
                FromCurrency = "EUR",
                ToCurrency = "USD",
                Date = DateTime.UtcNow,
                AskPrice = 1.01m,
                BidPrice = 1.02m,
            };

            _exchangeRatesRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(exchangeRate);

            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync($"/api/exchangeRates/{id}");
            var ExchangeRateDto = await response.Content.ReadFromJsonAsync<ExchangeRateDto>();

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            ExchangeRateDto.Should().NotBeNull();
            ExchangeRateDto.FromCurrency.Should().Be("EUR");
            ExchangeRateDto.ToCurrency.Should().Be("USD");
            ExchangeRateDto.AskPrice.Should().Be(1.01m);
            ExchangeRateDto.BidPrice.Should().Be(1.02m);
        }

        [Fact()]
        public async void GetAll_ForValidationRequest_Returns200Ok()
        {
            // arrange
            var client = _factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/exchangeRates?pageNumber=1&pageSize=10");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact()]
        public async void GetAll_ForInvalidRequest_Returns400BadRequest()
        {
            // arrange
            var client = _factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/exchangeRates");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}