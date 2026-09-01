namespace Identity.API.Common.Results.SuccessToResult;

public sealed class SuccessToResultMapper : ISuccessToResultMapper
{
    public IResult Map(SuccessResponse successResponse, object? value)
    {
        return successResponse.StatusCode switch
        {
            StatusCodes.Status200OK => Microsoft.AspNetCore.Http.Results.Ok(value),

            StatusCodes.Status201Created => Microsoft.AspNetCore.Http.Results.Created(successResponse.Location!, value),

            StatusCodes.Status204NoContent => Microsoft.AspNetCore.Http.Results.NoContent(),

            _ => throw new ArgumentOutOfRangeException(nameof(successResponse.StatusCode), successResponse,
                "Unsupported success response status code")
        };
    }
}