namespace Askify.Application.Users.Queries.GetResetPasswordForm;

public record GetResetPasswordFormQuery(string Token) : IRequest<string>;
