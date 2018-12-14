using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FriendlySpork.Models
{
    /// <summary>
    /// HttpContent with JSON-serialized object as its content.
    /// </summary>
    public sealed class ApplicationJsonContent : StringContent
    {
        public ApplicationJsonContent(object content) : base(GetContentString(content), Encoding.UTF8)
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        private static string GetContentString(object content)
        {
            return JsonConvert.SerializeObject(content);
        }
    }
}
