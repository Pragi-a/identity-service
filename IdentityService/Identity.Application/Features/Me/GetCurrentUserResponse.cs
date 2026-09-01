namespace Identity.Application.Features.Me;

public sealed record GetCurrentUserResponse(Guid UserId, string Email);