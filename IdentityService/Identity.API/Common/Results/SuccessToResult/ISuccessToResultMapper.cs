namespace Identity.API.Common.Results.SuccessToResult;

public interface ISuccessToResultMapper
{
    IResult Map(SuccessResponse successResponse, object? value);
}