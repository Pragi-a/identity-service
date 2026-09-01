using Identity.Application.Common.Results;
using MediatR;

namespace Identity.Application.Interfaces.Repositories;

public interface ITransactionalRequest<TResponse> : IRequest<Result<TResponse>>
{
    
}