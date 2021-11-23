using System.Text.Json.Serialization;

namespace Products.API.Dto.Infrastructure
{
    public class ImpersonationRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
