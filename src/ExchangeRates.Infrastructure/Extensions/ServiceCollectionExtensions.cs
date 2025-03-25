using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Interfaces;
using ExchangeRates.Domain.Repositories;
using ExchangeRates.Infrastructure.Authorization;
using ExchangeRates.Infrastructure.Authorization.Requirements;
using ExchangeRates.Infrastructure.Authorization.Services;
using ExchangeRates.Infrastructure.Integrations;
using ExchangeRates.Infrastructure.Persistence;
using ExchangeRates.Infrastructure.Repositories;
using ExchangeRates.Infrastructure.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRates.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ExchangeRatesDb");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString)
            .EnableSensitiveDataLogging());

        services.AddHttpClient<AlphaVantageClient>();

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IApplicationSeeder, ApplicationSeeder>();
        services.AddScoped<IExchangeRatesRepository, ExchangeRatesRepository>();
        services.AddScoped<IExternalExchangeRatesRepository, ExternalExchangeRatesRepository>();

        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasNationality, builder => builder.RequireClaim(AppClaimTypes.Nationality, "Portuguese", "Spanish"))
            .AddPolicy(PolicyNames.AtLeast18, builder => builder.AddRequirements(new MinimumAgeRequirement(18)));

        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        services.AddScoped<IApplicationAuthorizationService, ApplicationAuthorizationService>();

    }
}