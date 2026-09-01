namespace Identity.Application.Common.Errors.Helper;

public static class UserErrors
{
    public static readonly Error EmailAlreadyExists = Error.Conflict("email_already_exists", "Email is already in use");

    public static readonly Error InvalidCredentials =
        Error.Unauthenticated("invalid_credentials", "Invalid credentials");

    public static readonly Error UserNotFound = Error.NotFound("user_not_found", "User not found");

    public static readonly Error InvalidRefreshToken = Error.NotFound("invalid_refresh_token", "Invalid refresh token");

    public static readonly Error InvalidRole = Error.NotFound("invalid_role", "Selected Role is invalid");

    public static readonly Error AlreadyUsedRefreshToken =
        Error.Conflict("already_used_refresh_token", "Already used refresh token");
}