using Microsoft.Extensions.Logging;
using ExchangeRates.Application.Users;
using ExchangeRates.Domain.Constants;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Interfaces;

namespace ExchangeRates.Infrastructure.Authorization.Services;

public class ApplicationAuthorizationService(ILogger<ApplicationAuthorizationService> logger, IUserContext userContext)
    : IApplicationAuthorizationService
{
    public bool Authorize(ExchangeRate exchangeRate, ResourceOperation resourceOperation)
    {

        var user = userContext.GetCurrentUser();

        logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for Exchange Rate {ExchangeRateId}",
            user.Email, resourceOperation, exchangeRate.Id);

        if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/Read operation - successful authorization");
            return true;
        }

        if ((resourceOperation == ResourceOperation.Create ||
            resourceOperation == ResourceOperation.Read ||
            resourceOperation == ResourceOperation.Update ||
            resourceOperation == ResourceOperation.Delete)
            && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin user - successful authorization");
            return true;
        }

        return false;
    }
}
