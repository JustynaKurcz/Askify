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
                        throw new TokenExpiredException("Token has expired");
                    }
                }
            }
            catch (TokenExpiredException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidTokenException($"Invalid token: {ex.Message}");
            }
        }

        await next(context);
    }
}

public class TokenExpiredException : Exception
{
    public TokenExpiredException(string message) : base(message)
    {
    }
}

public class InvalidTokenException : Exception
{
    public InvalidTokenException(string message) : base(message)
    {
    }
}