using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Api.Core.Models;
using Api.Core.Models.Abstract;
using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.EntityFrameworkCore;
using NL.Kingscode.Flok.Storage.Api.Contexts;
using NL.Kingscode.Flok.Storage.Api.Requests.Authentication.Login;

namespace NL.Kingscode.Flok.Storage.Api.Controllers.Authentication
{
    [Route("api/auth/dispense")]
    [ApiController]
    [AllowAnonymous]
    public class DispenseController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public DispenseController(ApplicationContext context)
        {
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

        private static UserToken CreateToken([NotNull] User user)
        {
            return BaseToken.CreateForUser<UserToken>(user);
        }

        private static Uri GenerateRedirectUrl(string token, string redirectUri)
        {
            Env.Load();
            var envScheme = Env.GetString("SPA_SCHEME");
            if (!Enum.TryParse<HttpScheme>(envScheme, true, out var scheme))
                scheme = HttpScheme.Https;

            var url = new UriBuilder
            {
                Scheme = scheme.ToString(),
                Host = Env.GetString("SPA_HOST"),
                Port = Env.GetInt("SPA_PORT", 80),
                Path = Env.GetString("SPA_CALLBACK", "auth/callback"),
                Fragment = (string.IsNullOrWhiteSpace(token) ? null : "token=" + token)!,
                Query = "redirect_uri=" + redirectUri
            };

            return url.Uri;
        }
    }
}