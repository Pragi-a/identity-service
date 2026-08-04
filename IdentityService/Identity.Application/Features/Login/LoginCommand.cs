using Identity.Application.Common.Results;
using Identity.Domain.ValueObjects;
using MediatR;

namespace Identity.Application.Features.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;
