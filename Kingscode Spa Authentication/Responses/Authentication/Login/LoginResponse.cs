using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Nl.KingsCode.SpaAuthentication.Responses.Authentication.Login
{
    public sealed class LoginResponse
    {
        public LoginResponse(string token)
        {
            Token = token;
        }

        [Required] [NotNull] public string Token { get; }
    }
}