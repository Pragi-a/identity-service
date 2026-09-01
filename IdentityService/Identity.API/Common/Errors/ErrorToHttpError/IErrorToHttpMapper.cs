using Identity.Application.Common.Errors;

namespace Identity.API.Common.Errors.ErrorToHttpError;

public interface IErrorToHttpMapper
{
    HttpError Map(Error error);
}