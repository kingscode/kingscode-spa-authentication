using System;

namespace Nl.KingsCode.SpaAuthentication.Services
{
    public sealed class TokenGenerator
    {
        private readonly Random _random = new();

        public TokenGenerator(int length = 128)
        {
            Length = length % 2 == 1 ? length + 1 : length;
        }

        public int Length { get; }

        public string NextToken => GenerateToken();

        private string GenerateToken()
        {
            var buffer = new byte[Length / 2];
            _random.NextBytes(buffer);
            return BitConverter.ToString(buffer).Replace("-", "");
        }
    }
}