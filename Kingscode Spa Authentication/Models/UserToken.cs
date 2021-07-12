using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Core.Models.Abstract;

namespace Api.Core.Models
{
    public sealed class UserToken : BaseToken
    {
        public UserToken()
        {
            ExpiresAt = DateTime.Now.AddYears(1);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; init; }
    }
}