using MediatR;
using FluentValidation;
using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;

namespace Identity.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : notnull, IRequest<Result<TResponse>>
{
    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken cancellationToken)
    {
        var validationContext = new ValidationContext(request);

        var results = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(validationContext, cancellationToken))
        );

        var failures = results.SelectMany(x => x.Errors).ToList();

        if (failures.Count != 0)
        {
            var errors = failures
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => y.ErrorMessage).ToArray()
                );
            var validationError = new CustomValidationError(errors);

            return Result<TResponse>.Failure(validationError);
        }

        return await next(cancellationToken);
    }
}