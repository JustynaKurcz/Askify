namespace Askify.Shared.Results;

public sealed record Error(string Code, string? Message = null);