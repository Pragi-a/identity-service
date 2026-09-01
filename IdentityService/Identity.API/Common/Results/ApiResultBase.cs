using Identity.Application.Common.Results;

namespace Identity.API.Common.Results;

public abstract class ApiResultBase(Result result, SuccessResponse? successResponse)
{
    public Result Result { get; } = result;

    public SuccessResponse? SuccessResponse { get; } = successResponse;
}