using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Features.LogoutAll;

public sealed record LogoutAllCommand(string refreshToken): IRequest<Result<LogoutAllResponse>>;