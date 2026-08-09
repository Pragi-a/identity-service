namespace Identity.Application.Features.Users.GetUsers.Projections;

public sealed record UserListItem(Guid UserId, string Email,string FirstName, string LastName);