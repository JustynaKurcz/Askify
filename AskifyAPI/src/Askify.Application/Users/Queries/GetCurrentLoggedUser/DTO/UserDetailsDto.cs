namespace Askify.Application.Users.Queries.GetCurrentLoggedUser.DTO;

public record UserDetailsDto(
    Guid Id,
    string Email,
    string UserName,
    DateTimeOffset CreatedAt,
    string Role
);