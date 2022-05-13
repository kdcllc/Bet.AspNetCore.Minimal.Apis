using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bet.AspNetCore.Minimal.Api.Security.JwtBearer;

internal class JwtBearerGeneratorService : IJwtBearerGeneratorService
{
    private readonly JwtBearerSettings _options;

    public JwtBearerGeneratorService(IOptions<JwtBearerSettings> options)
    {
        _options = options.Value;
    }

    public string CreateToken(string username, IList<Claim>? claims = null, string? issuer = null, string? audience = null)
    {
        claims ??= new List<Claim>();
        if (!claims.Any(c => c.Type == ClaimTypes.Name))
        {
            claims.Add(new Claim(ClaimTypes.Name, username));
        }

        var signingCredentials = !string.IsNullOrWhiteSpace(_options.SecurityKey)
            ? new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecurityKey)), SecurityAlgorithms.HmacSha256)
            : null;

        var jwtSecurityToken = new JwtSecurityToken(
            issuer ?? _options.Issuers?.FirstOrDefault(),
            audience ?? _options.Audiences?.FirstOrDefault(),
            claims,
            DateTime.UtcNow,
            _options.ExpirationTime.GetValueOrDefault() > TimeSpan.Zero ? DateTime.UtcNow.Add(_options.ExpirationTime!.Value) : null,
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}
