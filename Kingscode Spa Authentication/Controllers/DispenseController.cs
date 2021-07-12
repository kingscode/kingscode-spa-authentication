using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.EntityFrameworkCore;
using Nl.KingsCode.SpaAuthentication.Interfaces;
using Nl.KingsCode.SpaAuthentication.Models;
using Nl.KingsCode.SpaAuthentication.Models.Abstract;
using Nl.KingsCode.SpaAuthentication.Requests.Authentication.Login;
using Nl.KingsCode.SpaAuthentication.Services;

namespace Nl.KingsCode.SpaAuthentication.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/auth/dispense")]
    [ApiController]
    [AllowAnonymous]
    public class DispenseController : ControllerBase
    {
        private readonly AuthenticationEnvironmentService _authenticationEnvironmentService;
        private readonly IAuthenticationContext _context;

        public DispenseController(AuthenticationEnvironmentService authenticationEnvironmentService,IAuthenticationContext context)
        {
            _authenticationEnvironmentService = authenticationEnvironmentService;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Dispense([FromForm] DispenseRequest request)
        {
            if (request == null) return BadRequest();

            var loginTokens = await _context.LoginTokens
                .Include(token => token.User)
                .Where(token => token.Token.Equals(request.Token))
                .Where(token => token.User.Email.Equals(request.Email))
                .ToListAsync();

            foreach (var userToken in loginTokens
                .Where(loginToken => !loginToken.IsExpired())
                .Select(loginToken => CreateToken(loginToken.User)))
            {
                _context.UserTokens.Add(userToken);
                await _context.SaveChangesAsync();
                return new RedirectResult(GenerateRedirectUrl(userToken.Token, request.RedirectUri).AbsoluteUri);
            }

            return new RedirectResult(GenerateRedirectUrl(null, request.RedirectUri).AbsoluteUri);
        }

        private static UserToken CreateToken([NotNull] IUser user)
        {
            return BaseToken.CreateForUser<UserToken>(user);
        }

        private  Uri GenerateRedirectUrl(string token, string redirectUri)
        {
            if (!Enum.TryParse<HttpScheme>(_authenticationEnvironmentService.SpaScheme, true, out var scheme))
                scheme = HttpScheme.Https;

            var url = new UriBuilder
            {
                Scheme = scheme.ToString(),
                Host =_authenticationEnvironmentService.SpaHost,
                Port =_authenticationEnvironmentService.SpaPort,
                Path =_authenticationEnvironmentService.SpaCallback,
                Fragment = string.IsNullOrWhiteSpace(token) ? string.Empty : "token=" + token,
                Query = "redirect_uri=" + redirectUri
            };

            return url.Uri;
        }
    }
}