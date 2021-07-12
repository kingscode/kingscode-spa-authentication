using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nl.KingsCode.SpaAuthentication.Models.Abstract;

namespace Nl.KingsCode.SpaAuthentication.Models
{
    public sealed class LoginToken : BaseToken
    {
        public LoginToken()
        {
            ExpiresAt = DateTime.Now.AddMinutes(1);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }
    }
}