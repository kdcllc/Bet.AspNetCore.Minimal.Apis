using System.Security.Claims;

namespace Bet.AspNetCore.Minimal.Api.Security.JwtBearer;

/// <summary>
/// Jwt token generator.
/// </summary>
public interface IJwtBearerGeneratorService
{
    /// <summary>
    /// Generates the token.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="claims"></param>
    /// <param name="issuer"></param>
    /// <param name="audience"></param>
    /// <returns></returns>
    string CreateToken(string username, IList<Claim>? claims = null, string? issuer = null, string? audience = null);
}
