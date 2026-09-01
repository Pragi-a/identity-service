namespace Identity.Application.Interfaces.Repositories.Queries;

public interface IUserAuthorizationQueries
{
    Task<IReadOnlyCollection<string>> GetPermissionCodesAsync(Guid userId,CancellationToken cancellationToken);
}