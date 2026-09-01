using Identity.Application.Common.Errors;
using Identity.Application.Common.Errors.Helper;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces;
using Identity.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence;

public sealed class UnitOfWork(IdentityDbContext identityDbContext, IPublisher publisher) : IUnitOfWork
{
    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var domainEvents = identityDbContext.ChangeTracker
                .Entries<Entity>()
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent, cancellationToken);
            }

            await identityDbContext.SaveChangesAsync(cancellationToken);

            foreach (var entry in identityDbContext.ChangeTracker.Entries<Entity>())
            {
                entry.Entity.ClearDomainEvents();
            }

            return Result.Success();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Result.Failure(CommonErrors.ConcurrencyConflict);
        }
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        var transaction = await identityDbContext.Database.BeginTransactionAsync(cancellationToken);

        return new EfCoreTransaction(transaction);
    }
}