using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nl.KingsCode.SpaAuthentication.Interfaces;
using Nl.KingsCode.SpaAuthentication.Models;
using Nl.KingsCode.SpaAuthentication.Models.Abstract;
using Nl.KingsCode.SpaAuthentication.Requests.Authentication.Login;
using Nl.KingsCode.SpaAuthentication.Responses;
using Nl.KingsCode.SpaAuthentication.Responses.Authentication.Login;

namespace Nl.KingsCode.SpaAuthentication.Controllers
{
    [Route("api/auth/login")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticationContext _context;
        private readonly PasswordHasher<IUser> _hasher;

        public LoginController(IAuthenticationContext context, PasswordHasher<IUser> hasher)
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