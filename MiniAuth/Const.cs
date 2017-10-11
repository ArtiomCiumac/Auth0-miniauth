namespace MiniAuth
{
    /// <summary>
    /// Constants to reuse across project for easier refactoring.
    /// </summary>
    public class Const
    {
        /// <summary>
        /// The name of the Auth0 authentication scheme.
        /// </summary>
        public const string Auth0 = "Auth0";

        /// <summary>
        /// Configuration key name for Auth0 IdP domain
        /// </summary>
        public const string ConfigDomainKey = "Auth0:Domain";

        /// <summary>
        /// Configuration key name for Auth0 API Client Id
        /// </summary>
        public const string ConfigClientIdKey = "Auth0:ClientId";

        /// <summary>
        /// Configuration key name for Auth0 API Client Secret
        /// </summary>
        public const string ConfigClientSecretKey = "Auth0:ClientSecret";
    }
}
