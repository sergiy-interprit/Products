using System;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Products.API.Infrastructure
{
    public interface IJwtAuthManager
    {
        JwtAuthResult GenerateJwtToken(string username, Claim[] claims, DateTime now);

        (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token);
    }
}
