using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Nl.KingsCode.SpaAuthentication.Interfaces;
using Nl.KingsCode.SpaAuthentication.Services;

namespace Nl.KingsCode.SpaAuthentication.Models.Abstract
{
    public abstract class BaseToken
    {
        [Required] [NotNull] public IUser User { get; private set; }
        [Required] [NotNull] public string Token { get; private set; }

        [Required] public DateTime ExpiresAt { get; set; }

        public bool IsExpired()
        {
            return ExpiresAt < DateTime.Now;
        }

        public static TToken CreateForUser<TToken>(IUser user)
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