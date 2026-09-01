namespace Identity.API.Features.Users.CreateUser;

public sealed record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    IReadOnlyCollection<Guid> RoleIds);

