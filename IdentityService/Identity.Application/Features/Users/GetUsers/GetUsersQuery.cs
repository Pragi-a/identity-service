using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.Users.GetUsers;

public sealed record GetUsersQuery(int PageSize, int CurrentPage) : IRequest<Result<GetUsersResponse>>;