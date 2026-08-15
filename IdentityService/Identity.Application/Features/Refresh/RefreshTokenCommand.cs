using Identity.Application.Common.Results;
using Identity.Application.Features.Login;
using Identity.Application.Interfaces.Repositories;
using MediatR;

namespace Identity.Application.Features.Refresh;

public sealed record RefreshTokenCommand(string RefreshToken) : ITransactionalRequest<LoginResponse>;
