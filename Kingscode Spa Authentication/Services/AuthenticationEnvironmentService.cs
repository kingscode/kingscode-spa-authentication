namespace Nl.KingsCode.SpaAuthentication.Services
{
    public class AuthenticationEnvironmentService
    {
        public AuthenticationEnvironmentService(
            string spaScheme,
            string spaHost,
            int spaPort = 80,
            string spaCallback = "auth/callback")
        {
            SpaScheme = spaScheme;
            SpaHost = spaHost;
            SpaPort = spaPort;
            SpaCallback = spaCallback;
        }

        public string SpaScheme { get; }
        public string SpaHost { get; }
        public int SpaPort { get; }
        public string SpaCallback { get; }
    }
}