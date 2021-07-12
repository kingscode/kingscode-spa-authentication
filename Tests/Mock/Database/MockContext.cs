using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Nl.KingsCode.SpaAuthentication.Interfaces;
using Nl.KingsCode.SpaAuthentication.Models;

namespace Tests.Mock.Database
{
    public class MockContext : DbContext, IAuthenticationContext
    {
        public MockContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<IUser> Users { get; }
        public DbSet<LoginToken> LoginTokens { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
    }
    
}