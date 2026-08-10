namespace Identity.Application.Common.Errors;

public static class CommonErrors
{
    public static readonly Error ConcurrencyConflict = new Error("concurrency_conflict","The resource was modified by another request.Please reload and try again");
}