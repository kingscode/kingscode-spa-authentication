using System.Linq;
using System.Threading.Tasks;
using Api.Core.Models;
using Api.Core.Models.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NL.Kingscode.Flok.Storage.Api.Contexts;
using NL.Kingscode.Flok.Storage.Api.Requests.Authentication.Login;
using NL.Kingscode.Flok.Storage.Api.Responses;
using NL.Kingscode.Flok.Storage.Api.Responses.Authentication.Login;

namespace NL.Kingscode.Flok.Storage.Api.Controllers.Authentication
{
    [Route("api/auth/login")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly PasswordHasher<User> _hasher;

        public LoginController(ApplicationContext context, PasswordHasher<User> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<LoginResponse>>> Login([FromForm] LoginRequest request)
        {
            if (request == null) return BadRequest();

            var users =
                from user in await _context.Users.Where(user => user.Email.Equals(request.Email)).ToListAsync()
                select user;

            foreach (var user in users)
            {
                var result = _hasher.VerifyHashedPassword(user, user.Password, request.Password);

                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (result == PasswordVerificationResult.SuccessRehashNeeded)
                    user.Password = _hasher.HashPassword(user, request.Password);

                if (result == PasswordVerificationResult.Failed) continue;

                var loginToken = BaseToken.CreateForUser<LoginToken>(user);
                _context.LoginTokens.Add(loginToken);
                await _context.SaveChangesAsync();

                var response = new LoginResponse(loginToken.Token);
                return new ActionResult<DataResponse<LoginResponse>>(new DataResponse<LoginResponse>(response));
            }

            return new NotFoundResult();
        }
    }
}