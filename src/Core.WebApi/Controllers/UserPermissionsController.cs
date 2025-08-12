using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Constants;

namespace Simpl.Expenses.WebAPI.Controllers;

[Route("api/userspermissions/{userId}/permissions")]
[ApiController]
[Authorize(Policy = PermissionCatalog.UsersManage)]
public class UserPermissionsController : ControllerBase
{
    private readonly IUserPermissionService _userPermissionService;

    public UserPermissionsController(IUserPermissionService userPermissionService)
    {
        _userPermissionService = userPermissionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserPermissionDto>>> GetPermissionsForUser(int userId, CancellationToken cancellationToken)
    {
        var permissions = await _userPermissionService.GetPermissionsForUserAsync(userId, cancellationToken);
        return Ok(permissions);
    }

    [HttpPost]
    public async Task<ActionResult<UserPermissionDto>> AddPermissionToUser(int userId, [FromBody] CreateUserPermissionDto createUserPermissionDto, CancellationToken cancellationToken)
    {
        if (userId != createUserPermissionDto.UserId)
        {
            return BadRequest("User ID in the route does not match User ID in the body.");
        }
        var createdPermission = await _userPermissionService.AddPermissionToUserAsync(createUserPermissionDto, cancellationToken);
        return Ok(createdPermission);
    }

    [HttpPut("{permissionId}")]
    public async Task<IActionResult> UpdatePermissionForUser(int userId, int permissionId, [FromBody] UpdateUserPermissionDto updateUserPermissionDto, CancellationToken cancellationToken)
    {
        await _userPermissionService.UpdatePermissionForUserAsync(userId, permissionId, updateUserPermissionDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{permissionId}")]
    public async Task<IActionResult> RemovePermissionFromUser(int userId, int permissionId, CancellationToken cancellationToken)
    {
        await _userPermissionService.RemovePermissionFromUserAsync(userId, permissionId, cancellationToken);
        return NoContent();
    }
}
