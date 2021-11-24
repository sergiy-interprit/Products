using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Products.API.Infrastructure;

namespace Products.IntegrationTests.Infrastructure
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
