using System.Text.Json.Serialization;

namespace Products.API.Infrastructure
{
    public class JwtAuthResult
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    }
}
