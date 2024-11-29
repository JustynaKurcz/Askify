namespace Askify.Shared.Emails;

public class EmailOptions
{
    public const string SectionName = "Email";
    
    public string SmtpHost { get; init; }
    public int SmtpPort { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string BaseUrl { get; init; } = "http://localhost:5244";
}