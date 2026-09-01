using Identity.Application.Common.Results;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Common.Behaviours;

public sealed class TransactionBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : ITransactionalRequest<TResponse>
{
    private async Task RollbackAsync(ITransaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            await transaction.RollbackAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to rollback transaction for request type  - {RequestType}",
                typeof(TRequest).Name);
        }
    }

    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        Result<TResponse> result;
        try
        {
            result = await next(cancellationToken);

            if (result.IsFailure)
            {
                await RollbackAsync(transaction, CancellationToken.None);
                return result;
            }

            var saveResult = await unitOfWork.SaveChangesAsync(cancellationToken);

            if (saveResult.IsFailure)
            {
                await RollbackAsync(transaction, CancellationToken.None);
                return Result<TResponse>.Failure(saveResult.Error);
            }
        }
        catch (Exception)
        {
            await RollbackAsync(transaction, CancellationToken.None);
            throw;
        }

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}