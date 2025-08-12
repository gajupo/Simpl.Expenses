using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;

namespace Simpl.Expenses.WebAPI.Controllers;

[Route("api/permissions")]
[ApiController]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetAllPermissions(CancellationToken cancellationToken)
    {
        var permissions = await _permissionService.GetAllPermissionsAsync(cancellationToken);
        return Ok(permissions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PermissionDto>> GetPermissionById(int id, CancellationToken cancellationToken)
    {
        var permission = await _permissionService.GetPermissionByIdAsync(id, cancellationToken);
        if (permission == null)
        {
            return NotFound();
        }
        return Ok(permission);
    }

    [HttpPost]
    public async Task<ActionResult<PermissionDto>> CreatePermission([FromBody] CreatePermissionDto createPermissionDto, CancellationToken cancellationToken)
    {
        var createdPermission = await _permissionService.CreatePermissionAsync(createPermissionDto, cancellationToken);
        return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.Id }, createdPermission);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePermission(int id, [FromBody] UpdatePermissionDto updatePermissionDto, CancellationToken cancellationToken)
    {
        await _permissionService.UpdatePermissionAsync(id, updatePermissionDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermission(int id, CancellationToken cancellationToken)
    {
        await _permissionService.DeletePermissionAsync(id, cancellationToken);
        return NoContent();
    }
}
