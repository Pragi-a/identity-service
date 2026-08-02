namespace Identity.Application.Common.Errors;

public static class UserErrors
{
    public static readonly Error EmailAlreadyExists = new Error("email_already_exists", "Email is already in use");
    
}