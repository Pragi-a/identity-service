using Identity.API.Common.Errors.ErrorToHttpError;
using Identity.API.Common.Errors.HttpErrorToResult;
using Identity.API.Common.Results;
using Identity.API.Common.Results.SuccessToResult;

namespace Identity.API.Common.Filters.EndpointFilters;

public sealed class ApiResultEndpointFilter(
    IErrorToHttpMapper errorToHttpMapper,
    IHttpErrorToResultMapper resultMapper,
    ISuccessToResultMapper successToResultMapper) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);

        if (result is not ApiResultBase apiResult)
        {
            throw new InvalidOperationException("The Endpoint must return an ApiResult");
        }

        if (apiResult.Result.IsFailure)
        {
            var httpError = errorToHttpMapper.Map(apiResult.Result.Error);
            return resultMapper.Map(httpError);
        }


        if (apiResult.SuccessResponse is null)
        {
            throw new InvalidOperationException("A successful ApiResult should contain a SuccessResponse");
        }

        var value = apiResult is IApiResultWithValue resultWithValue ? resultWithValue.Value : null;

        return successToResultMapper.Map(apiResult.SuccessResponse, value);
    }
}