using Askify.Application.Users.Commands.ResetPassword;

namespace Askify.Api.Endpoints.Users.Commands.ResetPassword;

internal sealed class ResetPasswordEndpointRequest
{
    [FromRoute(Name = "token")] public string Token { get; init; }
    [FromBody] public ResetPasswordCommand Command { get; init; } = default!;
}