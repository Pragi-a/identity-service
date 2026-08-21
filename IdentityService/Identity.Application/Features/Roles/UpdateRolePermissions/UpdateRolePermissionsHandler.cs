using Identity.Application.Common.Errors;
using Identity.Application.Common.Errors.Helper;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces;
using Identity.Application.Interfaces.Repositories.Commands;
using MediatR;

namespace Identity.Application.Features.Roles.UpdateRolePermissions;

public sealed class UpdateRolePermissionsHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateRolePermissionsCommand, Result<UpdateRolePermissionsResponse>>
{
    public async Task<Result<UpdateRolePermissionsResponse>> Handle(UpdateRolePermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.RoleId, cancellationToken);

        if (role == null)
            return Result<UpdateRolePermissionsResponse>.Failure(UserErrors.InvalidRole);

        role.ReplacePermissions(request.PermissionIds);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UpdateRolePermissionsResponse>.Success(new UpdateRolePermissionsResponse());
    }
}