using Identity.Application.Common.Results;

namespace Identity.API.Common.Results;

public sealed class ApiResult<T> : ApiResultBase, IApiResultWithValue
{
    private readonly Result<T> _result;
    public object Value => _result.Value!;

    private ApiResult(Result<T> result, SuccessResponse? successResponse)
        : base(result, successResponse)
    {
        _result = result;
    }

    public static ApiResult<T> Create(Result<T> result, Func<T, SuccessResponse> successResponseFactory)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(successResponseFactory);

        if (result.IsFailure)
        {
            return new ApiResult<T>(result, null);
        }

        var successResponse = successResponseFactory(result.Value);

        return new ApiResult<T>(result, successResponse);
    }
}

public sealed class ApiResult : ApiResultBase
{
    private ApiResult(
        Result result,
        SuccessResponse? successResponse)
        : base(result, successResponse)
    {
    }

    public static ApiResult Create(
        Result result,
        SuccessResponse successResponse)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(successResponse);

        return result.IsSuccess
            ? new ApiResult(result, successResponse)
            : new ApiResult(result, null);
    }
}