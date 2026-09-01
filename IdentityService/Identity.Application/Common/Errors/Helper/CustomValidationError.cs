namespace Identity.Application.Common.Errors.Helper;

public sealed record CustomValidationError(IReadOnlyDictionary<string, string[]> Errors)
    : Error.ValidationErrorBase("validation_error", "One or More Validation Errors occured")

{
    public override IReadOnlyDictionary<string, string[]> ValidationErrors  => Errors;
}