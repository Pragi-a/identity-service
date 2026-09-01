using FluentValidation;

namespace Identity.Application.Features.Users.GetUsers;

public sealed class GetUsersValidator
    : AbstractValidator<GetUsersQuery>
{
    public GetUsersValidator()
    {
        RuleFor(x => x.CurrentPage)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}