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
    public class UsoCFDIsController : ControllerBase
    {
        private readonly IUsoCFDIService _usoCFDIService;

        public UsoCFDIsController(IUsoCFDIService usoCFDIService)
        {
            _usoCFDIService = usoCFDIService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.UsoCFDIRead)]
        public async Task<IActionResult> GetUsoCFDIById(int id, CancellationToken cancellationToken = default)
        {
            var usoCFDI = await _usoCFDIService.GetUsoCFDIByIdAsync(id, cancellationToken);
            if (usoCFDI == null)
            {
                return NotFound();
            }
            return Ok(usoCFDI);
        }

        [HttpGet]
        [Authorize(Policy = PermissionCatalog.UsoCFDIRead)]
        public async Task<IActionResult> GetAllUsoCFDIs(CancellationToken cancellationToken = default)
        {
            var usoCFDIs = await _usoCFDIService.GetAllUsoCFDIsAsync(cancellationToken);
            return Ok(usoCFDIs);
        }

        [HttpPost]
        [Authorize(Policy = PermissionCatalog.UsoCFDICreate)]
        public async Task<IActionResult> CreateUsoCFDI([FromBody] CreateUsoCFDIDto createUsoCFDIDto, CancellationToken cancellationToken = default)
        {
            var newUsoCFDI = await _usoCFDIService.CreateUsoCFDIAsync(createUsoCFDIDto, cancellationToken);
            return CreatedAtAction(nameof(GetUsoCFDIById), new { id = newUsoCFDI.Id }, newUsoCFDI);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionCatalog.UsoCFDIUpdate)]
        public async Task<IActionResult> UpdateUsoCFDI(int id, [FromBody] UpdateUsoCFDIDto updateUsoCFDIDto, CancellationToken cancellationToken = default)
        {
            await _usoCFDIService.UpdateUsoCFDIAsync(id, updateUsoCFDIDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionCatalog.UsoCFDIDelete)]
        public async Task<IActionResult> DeleteUsoCFDI(int id, CancellationToken cancellationToken = default)
        {
            await _usoCFDIService.DeleteUsoCFDIAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
