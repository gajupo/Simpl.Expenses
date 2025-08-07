using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using System.Threading.Tasks;

namespace Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CostCentersController : ControllerBase
    {
        private readonly ICostCenterService _costCenterService;

        public CostCentersController(ICostCenterService costCenterService)
        {
            _costCenterService = costCenterService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCostCenterById(int id, CancellationToken cancellationToken = default)
        {
            var costCenter = await _costCenterService.GetCostCenterByIdAsync(id, cancellationToken);
            if (costCenter == null)
            {
                return NotFound();
            }
            return Ok(costCenter);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCostCenters(CancellationToken cancellationToken = default)
        {
            var costCenters = await _costCenterService.GetAllCostCentersAsync(cancellationToken);
            return Ok(costCenters);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCostCenter([FromBody] CreateCostCenterDto createCostCenterDto, CancellationToken cancellationToken = default)
        {
            var newCostCenter = await _costCenterService.CreateCostCenterAsync(createCostCenterDto, cancellationToken);
            return CreatedAtAction(nameof(GetCostCenterById), new { id = newCostCenter.Id }, newCostCenter);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCostCenter(int id, [FromBody] UpdateCostCenterDto updateCostCenterDto, CancellationToken cancellationToken = default)
        {
            await _costCenterService.UpdateCostCenterAsync(id, updateCostCenterDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCostCenter(int id, CancellationToken cancellationToken = default)
        {
            await _costCenterService.DeleteCostCenterAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
