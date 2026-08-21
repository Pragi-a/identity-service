using Identity.Application.Common.Errors;

namespace Identity.API.Common.Errors.ErrorToHttpError;

public sealed class ErrorToHttpMapper : IErrorToHttpMapper
{
    public HttpError Map(Error error)
    {
        return error.Classification switch
        {
            ErrorClassification.CONFLICT => new HttpError(StatusCodes.Status409Conflict, error.Code, "Conflict"),
            ErrorClassification.NOT_AUTHENTICATED => new HttpError(StatusCodes.Status401Unauthorized, error.Code,
                "Not Authenticated"),
            ErrorClassification.NOT_AUTHORIZED => new HttpError(StatusCodes.Status403Forbidden, error.Code,
                "Not Authorized"),
            ErrorClassification.NOT_FOUND => new HttpError(StatusCodes.Status404NotFound, error.Code,
                "Resource not Found"),
            ErrorClassification.VALIDATION when error is IValidationError validationError => new ValidationHttpError(
                StatusCodes.Status400BadRequest, error.Code, "Validation Failed", validationError.ValidationErrors),
            ErrorClassification.VALIDATION => throw new InvalidOperationException(
                "A Validation error must implement IValidationError"),

            ErrorClassification.NONE => throw new InvalidOperationException(
                "Error.None cannot be mapped to an HTTP error"),

            _ => throw new ArgumentOutOfRangeException(nameof(error.Classification), error.Classification, null)
        };
    }
}