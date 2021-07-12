using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nl.KingsCode.SpaAuthentication.Interfaces;

namespace Nl.KingsCode.SpaAuthentication.AuthenticationHandlers
{
    public sealed class KingscodeSpaAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthenticationContext _context;

        public KingscodeSpaAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthenticationContext context
        ) : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Skip authentication if endpoint has [AllowAnonymous] attribute
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

            if (string.IsNullOrWhiteSpace(authHeader.Parameter))
                return AuthenticateResult.Fail("Missing authorization token");

            var userToken = await _context.UserTokens
                .Include(token => token.User)
                .Where(token => token.Token.Equals(authHeader.Parameter))
                .FirstOrDefaultAsync();

            if (userToken == null) return AuthenticateResult.Fail("Invalid token provided");

            if (userToken.IsExpired()) return AuthenticateResult.Fail("Expired token");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userToken.User.Id.ToString()),
                new Claim(ClaimTypes.Name, userToken.User.Name)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}