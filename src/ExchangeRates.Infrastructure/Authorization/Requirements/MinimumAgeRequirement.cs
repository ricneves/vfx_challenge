using Microsoft.AspNetCore.Authorization;

namespace ExchangeRates.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirement(int minimumAge) : IAuthorizationRequirement
{
    public int MinimumAge { get; set; } = minimumAge;
}
