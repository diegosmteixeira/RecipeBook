using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeBook.Application.Services.Token;

public class TokenConfigurator
{
    private const string EmailAlias = "eml";
    private readonly double _tokenLifetime;
    private readonly string _tokenSecret;

    public TokenConfigurator(double tokenLifetime, string tokenSecret)
    {
        _tokenLifetime = tokenLifetime;
        _tokenSecret = tokenSecret;
    }

    public string GenerateToken(string email)
    {
        var claims = new List<Claim>
        {
            new Claim(EmailAlias, email)
        };

        var tokenHander = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_tokenLifetime),
            SigningCredentials = new SigningCredentials(SymmetricKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHander.CreateToken(tokenDescriptor);

        return tokenHander.WriteToken(securityToken);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHander = new JwtSecurityTokenHandler();

        var validateParams = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            IssuerSigningKey = SymmetricKey(),
            ClockSkew = new TimeSpan(0),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        var claims = tokenHander.ValidateToken(token, validateParams, out _);

        return claims;
    }

    public string EmailRecovery(string token)
    {
        var claims = ValidateToken(token);

        return claims.FindFirst(EmailAlias).Value;
    }

    private SymmetricSecurityKey SymmetricKey()
    {
        var symmetricKey = Encoding.UTF8.GetBytes(_tokenSecret);
        return new SymmetricSecurityKey(symmetricKey);
    }
}
