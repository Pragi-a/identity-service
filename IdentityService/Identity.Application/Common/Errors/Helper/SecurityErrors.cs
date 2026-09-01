namespace Identity.Application.Common.Errors.Helper;

public static class SecurityErrors
{
    public static readonly Error Unauthenticated = Error.Unauthenticated("Unauthenticated", "Authentication Required");

    public static readonly Error Forbidden =
        Error.Unauthorized("Forbidden", "You do not have permission to perform  this operation");
}