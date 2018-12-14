using System.Threading.Tasks;
using Auth0.ManagementApi;

namespace FriendlySpork
{
    public interface IAuth0ClientFactory
    {
        IManagementApiClient GetManagementApiClient();
        Task<IManagementApiClient> GetManagementApiClientAsync();
    }
}