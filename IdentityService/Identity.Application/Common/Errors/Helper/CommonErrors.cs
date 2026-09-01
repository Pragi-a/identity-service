namespace Identity.Application.Common.Errors.Helper;

public static class CommonErrors
{
    public static readonly Error ConcurrencyConflict = Error.Conflict("concurrency_conflict",
        "The resource was modified by another request.Please reload and try again");
}