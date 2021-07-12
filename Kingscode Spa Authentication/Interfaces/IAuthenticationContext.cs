using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nl.KingsCode.SpaAuthentication.Models;

namespace Nl.KingsCode.SpaAuthentication.Interfaces
{
    public interface IAuthenticationContext
    {
        DbSet<IUser> Users { get; }
        DbSet<LoginToken> LoginTokens { get; set; }
        DbSet<UserToken> UserTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}