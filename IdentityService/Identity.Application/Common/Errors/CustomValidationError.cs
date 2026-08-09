namespace Identity.Application.Common.Errors;

public sealed record CustomValidationError(IReadOnlyDictionary<string, string[]> ValidationErrors)
    : Error("validation_error", "One or More Validation Errors occured")
{
}