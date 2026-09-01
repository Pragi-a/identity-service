using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<Result<GetUserByIdResponse>>;