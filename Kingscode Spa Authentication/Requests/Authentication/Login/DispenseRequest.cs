using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Nl.KingsCode.SpaAuthentication.Requests.Authentication.Login
{
    public class DispenseRequest
    {
        [Required]
        [NotNull]
        [FromForm(Name = "email")]
        public string Email { get; set; }

        [Required]
        [NotNull]
        [FromForm(Name = "token")]
        public string Token { get; set; }

        [FromForm(Name = "redirect_uri")] public string RedirectUri { get; set; }
    }
}