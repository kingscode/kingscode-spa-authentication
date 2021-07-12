using Microsoft.EntityFrameworkCore;
using Tests.Mock.Database;

namespace Tests.Helpers
{
    public static class DatabaseHelper
    {
        public static DbContextOptions InMemoryDbContextBuilder(string name)
        {
            var builder = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(name)
                .EnableSensitiveDataLogging();

            return builder.Options;
        }

        public static MockContext CreateAppContext(string name)
        {
            var dbContext = InMemoryDbContextBuilder(name);

            var appContext = new MockContext(dbContext);
            appContext.Database.EnsureDeleted();
            appContext.Database.EnsureCreated();
            return appContext;
        }
    }
}