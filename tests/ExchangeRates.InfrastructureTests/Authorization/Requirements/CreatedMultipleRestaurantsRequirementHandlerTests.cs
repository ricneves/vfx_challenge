using ExchangeRates.Application.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExchangeRates.Infrastructure.Authorization.Requirements.Tests;

public class MinimumAgeRequirementHandlerTests
{
    [Fact()]
    public async Task HandleRequirementAsync_UserHasMinimumAge_ShouldSucceed()
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [], null, new DateOnly(2000, 01, 01));
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var loggerMock = new Mock<ILogger<MinimumAgeRequirementHandler>>();

        var requirement = new MinimumAgeRequirement(18);
        var handler = new MinimumAgeRequirementHandler(loggerMock.Object, userContextMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        // act 
        await handler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeTrue();

    }

    [Fact()]
    public async Task HandleRequirementAsync_UserHasNotMinimumAge_ShouldFail()
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [], null, new DateOnly(DateTime.Now.Year, 1, 1));
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

        var loggerMock = new Mock<ILogger<MinimumAgeRequirementHandler>>();

        var requirement = new MinimumAgeRequirement(18);
        var handler = new MinimumAgeRequirementHandler(loggerMock.Object, userContextMock.Object);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        // act 
        await handler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }
}