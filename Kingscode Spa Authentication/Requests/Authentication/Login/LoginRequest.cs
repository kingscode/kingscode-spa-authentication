using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace NL.Kingscode.Flok.Storage.Api.Requests.Authentication.Login
{
    public class LoginRequest
    {
        [Required]
        [NotNull]
        [FromForm(Name = "email")]
        public string Email { get; set; }

        [Required]
        [NotNull]
        [FromForm(Name = "password")]
        public string Password { get; set; }
    }
}