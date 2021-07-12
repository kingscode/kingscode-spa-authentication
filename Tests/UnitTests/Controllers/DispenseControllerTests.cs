using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nl.KingsCode.SpaAuthentication.Controllers;
using Nl.KingsCode.SpaAuthentication.Interfaces;
using Nl.KingsCode.SpaAuthentication.Models;
using Nl.KingsCode.SpaAuthentication.Models.Abstract;
using Nl.KingsCode.SpaAuthentication.Requests.Authentication.Login;
using NUnit.Framework;
using Tests.Helpers;
using Tests.Mock.Environment;

namespace Tests.UnitTests.Controllers
{
    public class DispenseControllerTests
    {
        private const string DefaultUserEmail = "dennis@kingscode.nl";
        private const string DefaultUserPassword = "test1234";

        private const string InvalidEmail = "koen@kingscode.nl";

        private IAuthenticationContext Context { get; set; }
        private DispenseController Controller { get; set; }
        private LoginToken LoginToken { get; set; }

        [Test]
        public async Task TestWithoutDispenseRequest()
        {
            var response = await Controller.Dispense(default);
            Assert.True(response is BadRequestResult);
        }

        [Test]
        public async Task TestDispenseWithInvalidUser()
        {
            var request = new DispenseRequest {Email = InvalidEmail};
            var response = await Controller.Dispense(request);

            var redirectResult = response as RedirectResult;
            Assert.True(redirectResult != null);
            Assert.False(redirectResult.Url.Contains('#'));
        }

        [Test]
        public async Task TestDispenseWithInvalidToken()
        {
            await CreateUser();
            var request = new DispenseRequest {Email = DefaultUserEmail};
            var response = await Controller.Dispense(request);

            var redirectResult = response as RedirectResult;
            Assert.True(redirectResult != null);
            Assert.False(redirectResult.Url.Contains('#'));
        }

        [Test]
        public async Task TestDispenseWithValidEmailAndToken()
        {
            await CreateUser();
            var request = new DispenseRequest {Email = DefaultUserEmail, Token = LoginToken.Token};
            var response = await Controller.Dispense(request);


            var redirectResult = response as RedirectResult;
            Assert.True(redirectResult != null);
            var token = redirectResult.Url.Split("#token=").Last();

            Assert.True(redirectResult.Url.Contains('#'));
            Assert.AreEqual(1, Context.UserTokens.Count());
            Assert.AreEqual(token, Context.UserTokens.First().Token);
        }

        [Test]
        public async Task TestExpiredTokenIsNotValid()
        {
            await CreateUser();
            LoginToken.ExpiresAt = DateTime.Now;
            await Context.SaveChangesAsync();

            var request = new DispenseRequest {Email = DefaultUserEmail, Token = LoginToken.Token};
            var response = await Controller.Dispense(request);

            var redirectResult = response as RedirectResult;
            Assert.True(redirectResult != null);
            Assert.False(redirectResult.Url.Contains('#'));
        }

        [SetUp]
        public void SetUp()
        {
            Context = DatabaseHelper.CreateAppContext(nameof(LoginControllerTests));
            Controller = new DispenseController(new MockAuthenticationEnvironmentService(), Context);
        }

        [TearDown]
        public void TearDown()
        {
            Controller = null;
            Context = null;
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Environment.SetEnvironmentVariable("SPA_HOST", "localhost");
        }

        private async Task CreateUser()
        {
            var user = new User
            {
                Email = DefaultUserEmail,
                Password = DefaultUserPassword
            };

            LoginToken = BaseToken.CreateForUser<LoginToken>(user);

            Context.Users.Add(user);
            Context.LoginTokens.Add(LoginToken);
            await Context.SaveChangesAsync();
        }
    }
}