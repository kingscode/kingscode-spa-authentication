using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Api.Core.Services;

namespace Api.Core.Models.Abstract
{
    public abstract class BaseToken
    {
        [Required] [NotNull] public User User { get; private set; }
        [Required] [NotNull] public string Token { get; private set; }

        [Required] public DateTime ExpiresAt { get; set; }

        public bool IsExpired()
        {
            return ExpiresAt < DateTime.Now;
        }

        public static TToken CreateForUser<TToken>(User user)
            where TToken : BaseToken, new()
        {
            return new()
            {
                User = user,
                Token = new TokenGenerator().NextToken
            };
        }
    }
}