using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExchangeRates.Domain.Constants;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Persistence;

namespace ExchangeRates.Infrastructure.Seeders
{
    internal class ApplicationSeeder(ApplicationDbContext dbContext) : IApplicationSeeder
    {
        public async Task Seed()
        {
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                await dbContext.Database.MigrateAsync();
            }

            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.ExchangeRates.Any())
                {
                    var exchangeRates = GetExchangeRates();
                    dbContext.ExchangeRates.AddRange(exchangeRates);
                    await dbContext.SaveChangesAsync();
                }

                if (!dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    dbContext.Roles.AddRange(roles);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<IdentityRole> GetRoles()
        {
            List<IdentityRole> roles =
                [
                new (UserRoles.User) { NormalizedName = UserRoles.User.ToUpper() },
                new (UserRoles.Admin) { NormalizedName = UserRoles.Admin.ToUpper() }
                ];
            return roles;
        }

        private IEnumerable<ExchangeRate> GetExchangeRates()
        {
            List<ExchangeRate> exchangeRates = [
                new()
                {
                    AskPrice = 1.01M,
                    BidPrice = 1.02M,
                    CreatedAt = new DateTime(2025,3,21),
                    CreatedBy = "seeder",
                    Date = new DateTime(2025,3,21),
                    FromCurrency = "EUR",
                    ToCurrency ="USD"
                },
                new ()
                {
                    AskPrice = 2.01M,
                    BidPrice = 2.02M,
                    CreatedAt = new DateTime(2021,1,1),
                    CreatedBy = "seeder",
                    Date = new DateTime(2021,1,1),
                    FromCurrency = "USD",
                    ToCurrency ="EUR"
                }
            ];

            return exchangeRates;
        }
    }
}
