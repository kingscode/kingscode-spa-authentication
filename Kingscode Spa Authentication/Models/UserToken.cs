using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nl.KingsCode.SpaAuthentication.Models.Abstract;

namespace Nl.KingsCode.SpaAuthentication.Models
{
    public sealed class UserToken : BaseToken
    {
        public UserToken()
        {
            ExpiresAt = DateTime.Now.AddYears(1);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }
    }
}