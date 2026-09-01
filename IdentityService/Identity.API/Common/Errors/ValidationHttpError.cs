namespace Identity.API.Common.Errors;

public sealed record ValidationHttpError(
    int StatusCode,
    string Code,
    string Title,
    IReadOnlyDictionary<string, string[]> ValidationErrors) : HttpError(StatusCode, Code, Title);