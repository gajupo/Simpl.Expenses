using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Application.Interfaces;
using System.Threading.Tasks;
using Simpl.Expenses.Domain.Constants;

namespace Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IAuthService _authService;

        public RolesController(IRoleService roleService, IAuthService authService)
        {
            _roleService = roleService;
            _authService = authService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.RolesManage)]
        public async Task<IActionResult> GetRoleById(int id, CancellationToken cancellationToken = default)
        {
            var role = await _roleService.GetRoleByIdAsync(id, cancellationToken);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpGet]
        [Authorize(Policy = PermissionCatalog.RolesManage)]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken = default)
        {
            var roles = await _roleService.GetAllRolesAsync(cancellationToken);
            return Ok(roles);
        }

        [HttpPost]
        [Authorize(Policy = PermissionCatalog.RolesManage)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto, CancellationToken cancellationToken = default)
        {
            var newRole = await _roleService.CreateRoleAsync(createRoleDto, cancellationToken);
            return CreatedAtAction(nameof(GetRoleById), new { id = newRole.Id }, newRole);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionCatalog.RolesManage)]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto updateRoleDto, CancellationToken cancellationToken = default)
        {
            await _roleService.UpdateRoleAsync(id, updateRoleDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionCatalog.RolesManage)]
        public async Task<IActionResult> DeleteRole(int id, CancellationToken cancellationToken = default)
        {
            await _roleService.DeleteRoleAsync(id, cancellationToken);
            return NoContent();
        }

        public record AssignPermissionsRequest(string[] PermissionNames);

        [HttpPost("{roleId}/permissions")]
        [Authorize(Policy = PermissionCatalog.PermissionsManage)]
        public async Task<IActionResult> AssignPermissionsToRole(int roleId, [FromBody] AssignPermissionsRequest request, CancellationToken cancellationToken = default)
        {
            await _authService.AssignPermissionsToRoleAsync(roleId, request.PermissionNames, cancellationToken);
            return NoContent();
        }
    }
}
