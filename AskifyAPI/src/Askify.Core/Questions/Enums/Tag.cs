using System.ComponentModel.DataAnnotations;

namespace Askify.Core.Questions.Enums;

public enum Tag
{
    [Display(Name = "Ogólne")] General = 1,

    [Display(Name = "Programowanie")] Programming = 2,

    [Display(Name = "Technologia")] Technology = 3,

    [Display(Name = "Nauka")] Science = 4,

    [Display(Name = "Matematyka")] Mathematics = 5,

    [Display(Name = "Biznes")] Business = 6,

    [Display(Name = "Edukacja")] Education = 7,

    [Display(Name = "Inne")] Other = 8
}