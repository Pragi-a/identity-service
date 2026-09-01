namespace Identity.Application.Features.Users.GetUsers.Projections;

public sealed record UserListResult(IReadOnlyCollection<UserListItem> UserListItems, int TotalCount);