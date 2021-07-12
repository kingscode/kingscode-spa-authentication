using System.Linq;
using System.Threading.Tasks;
using Api.Core.Models;
using Api.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NL.Kingscode.Flok.Storage.Api.Contexts;
using NL.Kingscode.Flok.Storage.Api.Controllers.Authentication;
using NL.Kingscode.Flok.Storage.Api.Requests.Authentication.Login;
using NUnit.Framework;

namespace Tests.UnitTests.Controllers
{
    public class LoginControllerTests
    {
        private const string ValidEmail = "dennis@kingscode.nl";
        private const string ValidPassword = "test1234";

        private const string InvalidEmail = "koen@kingscode.nl";
        private const string InvalidPassword = "4321tset";

        private ApplicationContext Context { get; set; }
        private PasswordHasher<User> Hasher { get; set; }
        private LoginController Controller { get; set; }

        [Test]
        public async Task TestWithoutLoginRequest()
        {
            var response = await Controller.Login(default);
            Assert.IsInstanceOf<BadRequestResult>(response.Result);
        }

        [Test]
        public async Task TestLoginWithInvalidEmail()
        {
            await CreateUser();
            var response = await Controller.Login(new LoginRequest
            {
                Email = InvalidEmail,
                Password = ValidPassword
            });
            Assert.IsInstanceOf<NotFoundResult>(response.Result);
        }

        [Test]
        public async Task TestLoginWithInvalidPassword()
        {
            await CreateUser();
            var response = await Controller.Login(new LoginRequest
            {
                Email = ValidEmail,
                Password = InvalidPassword
            });
            Assert.IsInstanceOf<NotFoundResult>(response.Result);
        }

        [Test]
        public async Task TestLoginWithValidCredentials()
        {
            await CreateUser();
            var response = await Controller.Login(new LoginRequest
            {
                Email = ValidEmail,
                Password = ValidPassword
            });
            Assert.Null(response.Result);
            Assert.NotNull(response.Value);
            Assert.NotNull(response.Value.Data);

            Assert.AreEqual(1, Context.LoginTokens.Count());
            Assert.AreEqual(Context.LoginTokens.First().Token, response.Value.Data.Token);
        }

        [SetUp]
        public void SetUp()
        {
            Context = DatabaseHelper.CreateAppContext(nameof(LoginControllerTests));
            Hasher = new PasswordHasher<User>();
            Controller = new LoginController(Context, new PasswordHasher<User>());
        }

        [TearDown]
        public void TearDown()
        {
            Controller = null;
            Context = null;
        }

        private async Task CreateUser()
        {
            var user = new User
            {
                Email = ValidEmail
            };
            user.Password = Hasher.HashPassword(user, ValidPassword);
            Context.Users.Add(user);
            await Context.SaveChangesAsync();
        }
    }
}