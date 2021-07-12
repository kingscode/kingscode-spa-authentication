using Microsoft.EntityFrameworkCore;
using NL.Kingscode.Flok.Storage.Api.Contexts;

namespace Api.Tests.Helpers
{
    public static class DatabaseHelper
    {
        public static DbContextOptions InMemoryDbContextBuilder(string name)
        {
            var builder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(name)
                .EnableSensitiveDataLogging(true);

            return builder.Options;
        }

        public static ApplicationContext CreateAppContext(string name)
        {
            var dbContext = InMemoryDbContextBuilder(name);

            var appContext = new ApplicationContext(dbContext);
            appContext.Database.EnsureDeleted();
            appContext.Database.EnsureCreated();
            return appContext;
        }
    }
}