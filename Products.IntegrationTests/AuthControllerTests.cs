using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Products.API.Infrastructure;
using Products.API.Dto.Infrastructure;
using Products.Domain.Constants;

namespace Products.IntegrationTests
{
    [TestClass]
    public class AuthControllerTests
    {
        private readonly TestHostFixture _testHostFixture = new TestHostFixture();
        private HttpClient _httpClient;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void SetUp()
        {
            _httpClient = _testHostFixture.Client;
            _serviceProvider = _testHostFixture.ServiceProvider;
        }

        [TestMethod]
        public async Task ShouldExpect401WhenLoginWithInvalidCredentials()
        {
            var credentials = new LoginRequest
            {
                Username = "admin",
                Password = "invalidPassword"
            };

            var response = await _httpClient.PostAsync("api/auth/login",
                new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task ShouldReturnCorrectResponseForSuccessLogin()
        {
            var credentials = new LoginRequest
            {
                Username = "admin",
                Password = "securePassword"
            };

            var loginResponse = await _httpClient.PostAsync("api/auth/login",
                new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResult>(loginResponseContent);
            
            Assert.AreEqual(credentials.Username, loginResult.Username);
            Assert.AreEqual(UserRoles.Admin, loginResult.Role);
            Assert.IsFalse(string.IsNullOrWhiteSpace(loginResult.AccessToken));

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            var (principal, jwtSecurityToken) = jwtAuthManager.DecodeJwtToken(loginResult.AccessToken);
            
            Assert.AreEqual(credentials.Username, principal.Identity.Name);
            Assert.AreEqual(UserRoles.Admin, principal.FindFirst(ClaimTypes.Role).Value);
            Assert.IsNotNull(jwtSecurityToken);
        }
    }
}
