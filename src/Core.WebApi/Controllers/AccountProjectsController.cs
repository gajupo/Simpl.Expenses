using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using System.Threading.Tasks;
using Simpl.Expenses.Domain.Constants;

namespace Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountProjectsController : ControllerBase
    {
        private readonly IAccountProjectService _accountProjectService;

        public AccountProjectsController(IAccountProjectService accountProjectService)
        {
            _accountProjectService = accountProjectService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.AccountProjectRead)]
        public async Task<IActionResult> GetAccountProjectById(int id, CancellationToken cancellationToken = default)
        {
            var accountProject = await _accountProjectService.GetAccountProjectByIdAsync(id, cancellationToken);
            if (accountProject == null)
            {
                return NotFound();
            }
            return Ok(accountProject);
        }

        [HttpGet]
        [Authorize(Policy = PermissionCatalog.AccountProjectRead)]
        public async Task<IActionResult> GetAllAccountProjects(CancellationToken cancellationToken = default)
        {
            var accountProjects = await _accountProjectService.GetAllAccountProjectsAsync(cancellationToken);
            return Ok(accountProjects);
        }

        [HttpPost]
        [Authorize(Policy = PermissionCatalog.AccountProjectCreate)]
        public async Task<IActionResult> CreateAccountProject([FromBody] CreateAccountProjectDto createAccountProjectDto, CancellationToken cancellationToken = default)
        {
            var newAccountProject = await _accountProjectService.CreateAccountProjectAsync(createAccountProjectDto, cancellationToken);
            return CreatedAtAction(nameof(GetAccountProjectById), new { id = newAccountProject.Id }, newAccountProject);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionCatalog.AccountProjectUpdate)]
        public async Task<IActionResult> UpdateAccountProject(int id, [FromBody] UpdateAccountProjectDto updateAccountProjectDto, CancellationToken cancellationToken = default)
        {
            await _accountProjectService.UpdateAccountProjectAsync(id, updateAccountProjectDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionCatalog.AccountProjectDelete)]
        public async Task<IActionResult> DeleteAccountProject(int id, CancellationToken cancellationToken = default)
        {
            await _accountProjectService.DeleteAccountProjectAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
