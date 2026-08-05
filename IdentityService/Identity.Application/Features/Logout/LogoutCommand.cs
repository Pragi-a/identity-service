using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.Logout;

public sealed record LogoutCommand(string refreshToken) :  IRequest<Result<LogoutResponse>>;