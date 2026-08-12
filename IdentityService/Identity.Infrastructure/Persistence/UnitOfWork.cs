
using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence;

public sealed class UnitOfWork(IdentityDbContext identityDbContext) : IUnitOfWork
{
    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await identityDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Result.Failure(CommonErrors.ConcurrencyConflict);
        }
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        var transaction =  await identityDbContext.Database.BeginTransactionAsync(cancellationToken);
        
        return new EfCoreTransaction(transaction);
    }
}