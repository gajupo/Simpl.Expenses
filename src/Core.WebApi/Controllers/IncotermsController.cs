using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using System.Threading.Tasks;

namespace Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncotermsController : ControllerBase
    {
        private readonly IIncotermService _incotermService;

        public IncotermsController(IIncotermService incotermService)
        {
            _incotermService = incotermService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncotermById(int id, CancellationToken cancellationToken = default)
        {
            var incoterm = await _incotermService.GetIncotermByIdAsync(id, cancellationToken);
            if (incoterm == null)
            {
                return NotFound();
            }
            return Ok(incoterm);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIncoterms(CancellationToken cancellationToken = default)
        {
            var incoterms = await _incotermService.GetAllIncotermsAsync(cancellationToken);
            return Ok(incoterms);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncoterm([FromBody] CreateIncotermDto createIncotermDto, CancellationToken cancellationToken = default)
        {
            var newIncoterm = await _incotermService.CreateIncotermAsync(createIncotermDto, cancellationToken);
            return CreatedAtAction(nameof(GetIncotermById), new { id = newIncoterm.Id }, newIncoterm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncoterm(int id, [FromBody] UpdateIncotermDto updateIncotermDto, CancellationToken cancellationToken = default)
        {
            await _incotermService.UpdateIncotermAsync(id, updateIncotermDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncoterm(int id, CancellationToken cancellationToken = default)
        {
            await _incotermService.DeleteIncotermAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
