namespace FriendlySpork
{
    /// <summary>
    /// Options required to generate Auth0 Management API Client instances.
    /// </summary>
    public sealed class Auth0FactoryOptions
    {
        /// <summary>
        /// Your Auth0 tenant domain.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Your Auth0 tenant client ID.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Your Auth0 tenant client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Whether to reuse access token when instantiating Management API Client.
        /// If true, an access token is reused until it is expired.
        /// If access token is never requested or expired, a new one is requested.
        /// </summary>
        public bool ReuseAccessToken { get; set; }
    }
}
