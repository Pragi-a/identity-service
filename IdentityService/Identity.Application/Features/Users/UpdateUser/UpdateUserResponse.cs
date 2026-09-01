namespace Identity.Application.Features.Users.UpdateUser;

public sealed record UpdateUserResponse(Guid UserId, string FirstName, string LastName, string Email);