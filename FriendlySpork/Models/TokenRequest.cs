namespace FriendlySpork.Models
{
    public sealed class TokenRequest
    {
        public string grant_type;
        public string client_id;
        public string client_secret;
        public string audience;
    }
}
