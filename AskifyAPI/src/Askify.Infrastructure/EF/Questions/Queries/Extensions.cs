using Askify.Application.Questions.Queries.BrowseQuestions.DTO;
using Askify.Application.Questions.Queries.GetQuestion.DTO;
using Askify.Core.Questions.Entities;
using Askify.Core.Questions.Enums;
using System.ComponentModel.DataAnnotations; 

namespace Askify.Infrastructure.EF.Questions.Queries;

internal static class Extensions
{
    public static QuestionDetailsDto AsDetailsDto(this Question question)
        => new(
            QuestionId: question.Id,
            Title: question.Title,
            Content: question.Content,
            UserId: question.UserId,
            CreatedAt: question.CreatedAt,
            Tag: question.Tag.GetDisplayName()
        );

    public static QuestionDto AsDto(this Question question)
        => new(
            QuestionId: question.Id,
            Title: question.Title,
            CreatedAt: question.CreatedAt,
            UserId: question.User.Id,
            Tag: question.Tag.GetDisplayName()
        );

    private static string GetDisplayName(this Tag tag)
    {
        var memberInfo = typeof(Tag).GetMember(tag.ToString())[0];
        var displayAttribute = memberInfo
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .FirstOrDefault() as DisplayAttribute;

        return displayAttribute?.Name ?? tag.ToString();
    }
}