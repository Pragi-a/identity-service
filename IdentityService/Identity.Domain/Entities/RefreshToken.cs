namespace Identity.Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; init; }

    public string TokenHash { get; init; }

    public Guid UserId { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime ExpiresAt { get; init; }

    public DateTime? RevokedAt { get; private set; }

    public Guid Version { get; private set; } = Guid.NewGuid();

    public bool IsRevoked => RevokedAt is not null;

    private RefreshToken(Guid id, string tokenHash, Guid userId, DateTime createdAt, DateTime expiresAt)
    {
        Id = id;
        TokenHash = tokenHash;
        UserId = userId;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }

    public bool CanBeUsed(DateTime utcNow)
    {
        return !IsRevoked && ExpiresAt > utcNow;
    }

    public void Revoke(DateTime utcNow)
    {
        if (IsRevoked) return;
        Version = Guid.NewGuid();
        this.RevokedAt = utcNow;
    }

    public static RefreshToken Create(string tokenHash, Guid userId, DateTime createdAt, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException("Invalid token hash for refresh token.");

        if (userId == Guid.Empty)
            throw new ArgumentException("Invalid user id.");

        if (expiresAt <= createdAt)
            throw new ArgumentException("Expiry date cannot be less than the created date");

        return new RefreshToken(Guid.NewGuid(), tokenHash, userId, createdAt, expiresAt);
    }
}