using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Askify.Shared.Auth;

public class AuthManager(IConfiguration configuration) : IAuthManager
{
    private readonly string _key = configuration["Authentication:JwtKey"]!;
    private readonly string _issuer = configuration["Authentication:Issuer"]!;
    private readonly string _audience = configuration["Authentication:Audience"]!;

    public JsonWebToken GenerateToken(Guid userId, string role)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, role)
        };

        var expires = DateTime.Now.AddHours(3);

        var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key!)),
                SecurityAlgorithms.HmacSha256)
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new JsonWebToken
        {
            AccessToken = token,
            RefreshToken = string.Empty,
            Expires = expires,
            Id = userId.ToString(),
            Role = role
        };
    }

    public string GeneratePasswordResetToken(string email)
    {
        var claims = new[]
        {
            new Claim("jti", Guid.NewGuid().ToString()),
            new Claim("purpose", "password_reset"),
            new Claim("email", email)
        };

        var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public bool VerifyPasswordResetToken(string token, out string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            email = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? string.Empty;

            return true;
        }
        catch
        {
            email = string.Empty;
            return false;
        }
    }
}