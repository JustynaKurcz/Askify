using Askify.Shared.Results;

namespace Askify.Shared.Auth.Middlewares;

public class TokenExpirationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var authorizationHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader["Bearer ".Length..].Trim();

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = handler.ReadJwtToken(token);

                var expirationDateUnix =
                    jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value;

                if (expirationDateUnix != null && long.TryParse(expirationDateUnix, out var expirationUnix))
                {
                    var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expirationUnix).UtcDateTime;

                    if (expirationDate < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        var error = new Error("TokenExpired", "Token has expired");
                        await context.Response.WriteAsJsonAsync(error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                var error = new Error("InvalidToken", $"Invalid token: {ex.Message}");
                await context.Response.WriteAsJsonAsync(error);
                return;
            }
        }

        await next(context);
    }
}