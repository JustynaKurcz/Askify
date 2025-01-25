using Askify.Application.Users.Queries.GetCurrentLoggedUser.DTO;
using Askify.Core.Users.Entities;
using Humanizer;
using Microsoft.OpenApi.Extensions;

namespace Askify.Infrastructure.EF.Users.Queries;

public static class Extensions
{
    internal static UserDetailsDto AsDetailsDto(this User user)
        => new(
            user.Id,
            user.Email,
            user.UserName,
            user.CreatedAt,
            user.Role.Humanize()
        );
}