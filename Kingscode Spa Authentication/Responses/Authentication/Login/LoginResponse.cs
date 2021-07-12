using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NL.Kingscode.Flok.Storage.Api.Responses.Authentication.Login
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