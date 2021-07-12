using Nl.KingsCode.SpaAuthentication.Services;
using NUnit.Framework;

namespace Tests.UnitTests.Services
{
    public class TokenGeneratorTests
    {
        [Test]
        public void TestTokenGeneratorGeneratesString()
        {
            var tokenGenerator = new TokenGenerator();
            var token = tokenGenerator.NextToken;
            var token2 = tokenGenerator.NextToken;

            Assert.NotNull(token);
            Assert.False(string.IsNullOrWhiteSpace(token2));
        }

        [Test]
        public void TestTokenGeneratorGeneratesRandomData()
        {
            var tokenGenerator = new TokenGenerator();
            var token1 = tokenGenerator.NextToken;
            var token2 = tokenGenerator.NextToken;

            Assert.AreNotEqual(token1, token2);
        }

        [Test]
        public void TestTokenGeneratorGeneratesTokenOfDesiredLength()
        {
            const int tokenLength = 256;
            var tokenGenerator = new TokenGenerator(tokenLength);
            var token = tokenGenerator.NextToken;

            Assert.AreEqual(tokenLength, token.Length);
        }

        [Test]
        public void TestTokenGeneratorGeneratesTokenOfEvenLengthWithUnevenLengthRequested()
        {
            const int tokenLength = 63;
            var tokenGenerator = new TokenGenerator(tokenLength);
            var token = tokenGenerator.NextToken;

            Assert.AreEqual(tokenLength + 1, token.Length);
        }
    }
}