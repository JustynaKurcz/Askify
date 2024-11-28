using System.ComponentModel.DataAnnotations;

namespace Askify.Core.Questions.Enums;

public enum Tag
{
    [Display(Name = "Ogólne")] General,

    [Display(Name = "Programowanie")] Programming,

    [Display(Name = "Technologia")] Technology,

    [Display(Name = "Nauka")] Science,

    [Display(Name = "Matematyka")] Mathematics,

    [Display(Name = "Biznes")] Business,

    [Display(Name = "Edukacja")] Education,

    [Display(Name = "Inne")] Other
}