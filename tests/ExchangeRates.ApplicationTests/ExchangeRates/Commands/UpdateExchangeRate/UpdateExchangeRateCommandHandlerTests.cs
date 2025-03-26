using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Exceptions;
using ExchangeRates.Domain.Interfaces;
using ExchangeRates.Domain.Repositories;
using Xunit;

namespace ExchangeRates.Application.ExchangeRates.Commands.UpdateExchangeRate.Tests;

public class UpdateExchangeRateCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateExchangeRateCommandHandler>> _loggerMock;
    private readonly Mock<IExchangeRatesRepository> _exchangeRatesRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IApplicationAuthorizationService> _applicationAuthorizationServiceMock;

    private readonly UpdateExchangeRateCommandHandler _handler;

    public UpdateExchangeRateCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateExchangeRateCommandHandler>>();
        _exchangeRatesRepositoryMock = new Mock<IExchangeRatesRepository>();
        _mapperMock = new Mock<IMapper>();
        _applicationAuthorizationServiceMock = new Mock<IApplicationAuthorizationService>();

        _handler = new UpdateExchangeRateCommandHandler(_loggerMock.Object,
            _exchangeRatesRepositoryMock.Object,
            _mapperMock.Object,
            _applicationAuthorizationServiceMock.Object);
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateExchangeRate()
    {
        // arrange
        var exchangeId = 1;

        var command = new UpdateExchangeRateCommand()
        {
            Id = exchangeId,
            AskPrice = 1.01m,
            BidPrice = 1.03m
        };

        var exchangeRate = new ExchangeRate()
        {
            Id = exchangeId,
            AskPrice = 2.01m,
            BidPrice = 2.03m
        };

        _exchangeRatesRepositoryMock.Setup(r => r.GetByIdAsync(exchangeId)).ReturnsAsync(exchangeRate);

        _applicationAuthorizationServiceMock.Setup(a => a.Authorize(exchangeRate, Domain.Constants.ResourceOperation.Update))
            .Returns(true);

        // act
        await _handler.Handle(command, CancellationToken.None);

        // assert
        _exchangeRatesRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, exchangeRate), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithNonExistingExchangeRate_ShouldThrowNotFoundException()
    {
        // arrange
        var exchangeId = 2;

        var request = new UpdateExchangeRateCommand()
        {
            Id = exchangeId
        };

        _exchangeRatesRepositoryMock.Setup(r => r.GetByIdAsync(exchangeId)).ReturnsAsync((ExchangeRate?)null);

        // act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"ExchangeRate with id: {exchangeId} doesn't exist");
    }

    [Fact()]
    public async Task Handle_WithUnauthorizedUser_ShouldThrowForbidException()
    {
        // arrange
        var exchangeId = 3;

        var request = new UpdateExchangeRateCommand
        {
            Id = exchangeId
        };

        var existingExchangeRate = new ExchangeRate
        {
            Id = exchangeId
        };

        _exchangeRatesRepositoryMock.Setup(r => r.GetByIdAsync(exchangeId)).ReturnsAsync(existingExchangeRate);

        _applicationAuthorizationServiceMock.Setup(a => a.Authorize(existingExchangeRate, Domain.Constants.ResourceOperation.Update))
            .Returns(false);

        // act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // assert
        await act.Should().ThrowAsync<ForbidException>();
    }
}