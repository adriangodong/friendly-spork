using System.Threading.Tasks;

namespace FriendlySpork
{
    public interface IAuth0AccessTokenFactory
    {
        string GetAccessToken();
        Task<string> GetAccessTokenAsync();
    }
}