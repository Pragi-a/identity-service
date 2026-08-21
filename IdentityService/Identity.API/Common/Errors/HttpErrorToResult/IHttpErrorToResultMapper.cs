namespace Identity.API.Common.Errors.HttpErrorToResult;

public interface IHttpErrorToResultMapper
{
    IResult Map(HttpError error);
}