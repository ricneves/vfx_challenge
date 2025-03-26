using FluentAssertions;
using ExchangeRates.Domain.Constants;
using Xunit;

namespace ExchangeRates.Application.Users.Tests;

public class CurrentUserTests
{
    // TestMethod_Scenario_ExpectResult

    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRoleTest_WithMatchingRole_ShoulReturnTrue(string roleName)
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act
        var isInRole = currentUser.IsInRole(roleName);

        // Assert
        isInRole.Should().BeTrue();
    }

    [Fact()]
    public void IsInRoleTest_WithNoMatchingRole_ShoulReturnFalse()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin], null, null);

        // Act
        var isInRole = currentUser.IsInRole(UserRoles.User);

        // Assert
        isInRole.Should().BeFalse();
    }

    [Fact()]
    public void IsInRoleTest_WithNoMatchingRoleCase_ShoulReturnFalse()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // Act
        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        // Assert
        isInRole.Should().BeFalse();
    }
}