namespace Askify.Application.Users.Queries.DTO;

public record UserDetailsDto(
    Guid Id,
    string Email,
    string UserName,
    DateTimeOffset CreatedAt,
    string Role
);