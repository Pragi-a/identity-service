namespace Identity.API.Common.Errors;

public record HttpError(int StatusCode, string Code, string Title);