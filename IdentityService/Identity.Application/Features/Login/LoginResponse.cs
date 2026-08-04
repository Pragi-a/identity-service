namespace Identity.Application.Features.Login;

public sealed record LoginResponse(string AccessToken, string RefreshToken, string AccessTokenExpiresAt);