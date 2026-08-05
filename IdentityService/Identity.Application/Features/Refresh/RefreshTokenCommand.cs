using Identity.Application.Common.Results;
using Identity.Application.Features.Login;
using MediatR;

namespace Identity.Application.Features.Refresh;

public sealed record RefreshTokenCommand(string refreshToken) : IRequest<Result<LoginResponse>>;
