namespace Identity.Application.Common.Errors;

public interface IValidationError
{
    IReadOnlyDictionary<string, string[]> ValidationErrors { get; }
}