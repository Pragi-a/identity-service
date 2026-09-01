namespace Identity.API.Features.Users.UpdateUser;

public sealed record UpdateUserRequest(
    string Email,
    string FirstName,
    string LastName,
    IReadOnlyCollection<Guid> RoleIds);
