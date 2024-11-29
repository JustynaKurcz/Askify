using Askify.Core.Users.Repositories;
using Askify.Shared.Auth;

namespace Askify.Api.Endpoints.Users.Commands.ResetPassword;

public sealed class ResetPasswordEndpoint : IEndpointDefinition
{
    private const string ResetPasswordFormPath = "Askify.Shared.Emails.Templates.ResetPasswordForm.html";
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"{UserEndpoints.BasePath}/reset-password/{{token}}", async (
            [FromRoute] string token,
            [FromServices] IAuthManager authManager,
            [FromServices] IUserRepository userRepository,
            CancellationToken cancellationToken) =>
        {
            if (!authManager.VerifyPasswordResetToken(token, out var email))
            {
                return Results.BadRequest("Invalid password reset token.");
            }

            var user = await userRepository.GetByEmailAsync(email, cancellationToken);
            if (user is null)
            {
                return Results.BadRequest("User not found.");
            }

            var assembly = typeof(IAuthManager).Assembly;
            
            using var stream = assembly.GetManifestResourceStream(ResetPasswordFormPath);
            if (stream == null)
            {
                throw new InvalidOperationException($"Could not find embedded resource '{ResetPasswordFormPath}'");
            }
            
            using var reader = new StreamReader(stream);
            var formHtml = await reader.ReadToEndAsync();
            
            formHtml = formHtml.Replace("{token}", token);
            formHtml = formHtml.Replace("{email}", user.Email);

            return Results.Content(formHtml, "text/html");
            
        });
        
        endpoints.MapPost($"{UserEndpoints.BasePath}/reset-password/{{token}}", async (
               [AsParameters] ResetPasswordEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(request.Command, cancellationToken);

                return Results.Ok();
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Reset password",
                Description = "Reset user password using token from email",
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
    
   
}