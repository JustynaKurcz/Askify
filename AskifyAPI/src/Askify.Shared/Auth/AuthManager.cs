using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Askify.Shared.Auth;

public class AuthManager(IConfiguration configuration) : IAuthManager
{
    public JsonWebToken GenerateToken(Guid userId, string role)
    {
        var key = configuration.GetSection("Authentication:JwtKey").Get<string>();
        var issuer = configuration.GetSection("Authentication:Issuer").Get<string>();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, role)
        };

        var expires = DateTime.Now.AddHours(3);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
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
}