using System.ComponentModel.DataAnnotations;

namespace Askify.Core.Users.Enums;

public enum Role : short
{
    [Display(Name = "Użytkownik")] User = 1,
    [Display(Name = "Administrator")] Admin = 2,
}