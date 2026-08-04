using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.Me;

public sealed record GetCurrentUserQuery(Guid UserId) : IRequest<Result<GetCurrentUserResponse>>;