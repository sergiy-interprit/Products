using System.Text.Json.Serialization;

namespace Products.API.Dto.Infrastructure
{
    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
