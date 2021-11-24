using System.Text.Json.Serialization;

namespace Products.API.Dto.Infrastructure
{
    public class LoginResult
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    }
}
