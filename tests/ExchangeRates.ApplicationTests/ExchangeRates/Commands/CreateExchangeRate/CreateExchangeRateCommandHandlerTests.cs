using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ExchangeRates.Application.Users;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Repositories;
using Xunit;

namespace ExchangeRates.Application.ExchangeRates.Commands.CreateExchangeRate.Tests;

public class CreateExchangeRateCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ReturnsCreatedExchangeRateId()
    {
        // arrange
        var loggerMock = new Mock<ILogger<CreateExchangeRateCommandHandler>>();
        var mapperMock = new Mock<IMapper>();

        var command = new CreateExchangeRateCommand();
        var exchangeRate = new ExchangeRate();

        mapperMock.Setup(m => m.Map<ExchangeRate>(command)).Returns(exchangeRate);

        var exchangeRateRepositoryMock = new Mock<IExchangeRatesRepository>();

        exchangeRateRepositoryMock.Setup(r => r.Create(It.IsAny<ExchangeRate>())).ReturnsAsync(1);

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("user-id", "test@test.com", [], null, null);
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var commandHandler = new CreateExchangeRateCommandHandler(
            loggerMock.Object,
            mapperMock.Object,
            exchangeRateRepositoryMock.Object,
            userContextMock.Object);

        // act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // assert
        result.Should().Be(1);
        exchangeRate.CreatedBy.Should().Be("user-id");
        exchangeRateRepositoryMock.Verify(r => r.Create(exchangeRate), Times.Once);
    }
}