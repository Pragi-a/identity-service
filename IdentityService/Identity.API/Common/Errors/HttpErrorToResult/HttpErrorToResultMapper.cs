namespace Identity.API.Common.Errors.HttpErrorToResult;

public sealed class HttpErrorToResultMapper : IHttpErrorToResultMapper
{
    public IResult Map(HttpError error)
    {
        var problemDetails = error switch
        {
            ValidationHttpError validationError => Microsoft.AspNetCore.Http.Results.ValidationProblem(
                errors: new Dictionary<string, string[]>(validationError.ValidationErrors),
                statusCode: validationError.StatusCode,
                title: validationError.Title,
                extensions: new Dictionary<string, object?>()
                {
                    ["code"] = validationError.Code
                }
            ),

            _ => Microsoft.AspNetCore.Http.Results.Problem(
                statusCode: error.StatusCode,
                title: error.Title,
                extensions: new Dictionary<string, object?>()
                {
                    ["code"] = error.Code
                }
            )
        };

        return problemDetails;
    }
}