using Identity.Application.Features.Users.GetUsers.Projections;

namespace Identity.Application.Features.Users.GetUsers;

public sealed record GetUsersResponse(IReadOnlyCollection<UserListItem> Users, int Page, int PageSize, int TotalCount);