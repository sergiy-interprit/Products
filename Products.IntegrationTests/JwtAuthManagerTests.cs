using System;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Products.API.Infrastructure;
using Products.API.Services;

namespace Products.IntegrationTests
{
    [TestClass]
    public class JwtAuthManagerTests
    {
        private readonly TestHostFixture _testHostFixture = new TestHostFixture();
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void SetUp()
        {
            _serviceProvider = _testHostFixture.ServiceProvider;
        }

        [TestMethod]
        public void ShouldLoadCorrectJwtConfig()
        {
            var jwtConfig = _serviceProvider.GetRequiredService<JwtTokenConfig>();
            Assert.AreEqual("1234567890123456789", jwtConfig.Secret);
            Assert.AreEqual(20, jwtConfig.AccessTokenExpiration);
        }
    }
}
