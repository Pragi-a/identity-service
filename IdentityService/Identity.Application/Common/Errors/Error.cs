namespace Identity.Application.Common.Errors;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new Error(string.Empty, string.Empty);
}