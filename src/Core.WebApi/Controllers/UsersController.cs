
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Application.Interfaces;
using System.Threading.Tasks;
using Simpl.Expenses.Domain.Constants;
using System.Security.Claims;

namespace Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken = default)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var userProfile = await _userService.GetUserProfileAsync(userId, cancellationToken);
            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.UsersManage)]
        public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Policy = PermissionCatalog.UsersManage)]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var users = await _userService.GetAllUsersAsync(cancellationToken);
            return Ok(users);
        }

        [HttpPost]
        [Authorize(Policy = PermissionCatalog.UsersManage)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto, CancellationToken cancellationToken = default)
        {
            var newUser = await _userService.CreateUserAsync(createUserDto, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionCatalog.UsersManage)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
        {
            await _userService.UpdateUserAsync(id, updateUserDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionCatalog.UsersManage)]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken = default)
        {
            await _userService.DeleteUserAsync(id, cancellationToken);
            return NoContent();
        }

        public record AssignRolesRequest(int[] RoleIds);
        public record SetPermissionsRequest(string[] PermissionNames);

        [HttpPost("{userId}/roles")]
        [Authorize(Policy = PermissionCatalog.UsersManage)]
        public async Task<IActionResult> AssignRoles(int userId, [FromBody] AssignRolesRequest request, CancellationToken cancellationToken = default)
        {
            await _authService.AssignRolesToUserAsync(userId, request.RoleIds, cancellationToken);
            return NoContent();
        }

        [HttpPost("{userId}/permissions")]
        [Authorize(Policy = PermissionCatalog.PermissionsManage)]
        public async Task<IActionResult> SetDirectPermissions(int userId, [FromBody] SetPermissionsRequest request, CancellationToken cancellationToken = default)
        {
            await _authService.SetDirectPermissionsForUserAsync(userId, request.PermissionNames, cancellationToken);
            return NoContent();
        }
    }
}
