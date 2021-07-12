using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Core.Models.Abstract;

namespace Api.Core.Models
{
    public sealed class LoginToken : BaseToken
    {
        public LoginToken()
        {
            ExpiresAt = DateTime.Now.AddMinutes(1);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; init; }
    }
}