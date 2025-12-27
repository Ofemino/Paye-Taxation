namespace Paye.Infrastructure.Configuration
{
    public class AuthenticationSettings
    {
        public bool UseExternal { get; set; }
    }

    public class ExternalAuthSettings
    {
        public bool IsEnabled { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
        public bool AutoSyncRoles { get; set; }
        public bool AutoCreateUsers { get; set; }
    }
}
