using Nl.KingsCode.SpaAuthentication.Services;

namespace Tests.Mock.Environment
{
    public sealed class MockAuthenticationEnvironmentService : AuthenticationEnvironmentService
    {
        public MockAuthenticationEnvironmentService() :
            base("http", "localhost", 8080)
        {
        }
    }
}