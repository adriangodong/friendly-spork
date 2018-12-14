namespace FriendlySpork
{
    public sealed class Auth0FactoryOptions
    {
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public bool ReuseAccessToken { get; set; }
    }
}
