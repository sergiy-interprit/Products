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
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Products.API.Dto;
using FluentAssertions;
using System.Collections.Generic;

namespace Products.IntegrationTests.Controllers
{
    [TestClass]
    public class AccountsControllerTests
    {
        private readonly TestHostFixture _testHostFixture = new TestHostFixture();
        private HttpClient _httpClient;
        private IServiceProvider _serviceProvider;

        private LoginRequest _adminCredentials = new LoginRequest
        {
            Username = "admin",
            Password = "securePassword"
        };

        private LoginRequest _userCredentials = new LoginRequest
        {
            Username = "test1",
            Password = "password1"
        };

        [TestInitialize]
        public void SetUp()
        {
            _httpClient = _testHostFixture.Client;
            _serviceProvider = _testHostFixture.ServiceProvider;
        }

        [TestMethod]
        public async Task ShouldExpect401WhenNotLoggedIn()
        {
            var response = await _httpClient.GetAsync("api/accounts");
            
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task ShouldGetAllAccountsUsingSuccessUserLogin()
        {
            var loginResponse = await _httpClient.PostAsync("api/auth/login",
                new StringContent(JsonSerializer.Serialize(_userCredentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResult>(loginResponseContent);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            var response = await _httpClient.GetAsync("api/accounts");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var createdAccountDtos = JsonSerializer.Deserialize<IEnumerable<AccountDto>>(content,
               new JsonSerializerOptions
               {
                   PropertyNamingPolicy = JsonNamingPolicy.CamelCase
               });

            createdAccountDtos.Should().HaveCountGreaterThan(2);
        }

        [TestMethod]
        public async Task ShouldCreateAccountUsingSuccessAdminLogin()
        {
            var loginResponse = await _httpClient.PostAsync("api/auth/login",
                new StringContent(JsonSerializer.Serialize(_adminCredentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResult>(loginResponseContent);

            var saveAccountDto = new SaveAccountDto()
            {
                Name = "Account3",
                Description = "Description of Account3."
            };

             var serializedSaveAccountDtoCreate = JsonSerializer.Serialize(saveAccountDto);

            var request = new HttpRequestMessage(HttpMethod.Post, "api/accounts");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);

            request.Content = new StringContent(serializedSaveAccountDtoCreate);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var createdAccountDto = JsonSerializer.Deserialize<AccountDto>(content,
               new JsonSerializerOptions
               {
                   PropertyNamingPolicy = JsonNamingPolicy.CamelCase
               });

            createdAccountDto.Id.Should().BePositive();
            Assert.AreEqual(saveAccountDto.Name, createdAccountDto.Name);
            Assert.AreEqual(saveAccountDto.Description, createdAccountDto.Description);
        }
    }
}
