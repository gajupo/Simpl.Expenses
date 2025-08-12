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
    public class PlantsController : ControllerBase
    {
        private readonly IPlantService _plantService;

        public PlantsController(IPlantService plantService)
        {
            _plantService = plantService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.PlantRead)]
        public async Task<IActionResult> GetPlantById(int id, CancellationToken cancellationToken = default)
        {
            var plant = await _plantService.GetPlantByIdAsync(id, cancellationToken);
            if (plant == null)
            {
                return NotFound();
            }
            return Ok(plant);
        }

        [HttpGet]
        [Authorize(Policy = PermissionCatalog.PlantRead)]
        public async Task<IActionResult> GetAllPlants(CancellationToken cancellationToken = default)
        {
            var plants = await _plantService.GetAllPlantsAsync(cancellationToken);
            return Ok(plants);
        }

        [HttpPost]
        [Authorize(Policy = PermissionCatalog.PlantCreate)]
        public async Task<IActionResult> CreatePlant([FromBody] CreatePlantDto createPlantDto, CancellationToken cancellationToken = default)
        {
            var newPlant = await _plantService.CreatePlantAsync(createPlantDto, cancellationToken);
            return CreatedAtAction(nameof(GetPlantById), new { id = newPlant.Id }, newPlant);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionCatalog.PlantUpdate)]
        public async Task<IActionResult> UpdatePlant(int id, [FromBody] UpdatePlantDto updatePlantDto, CancellationToken cancellationToken = default)
        {
            await _plantService.UpdatePlantAsync(id, updatePlantDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionCatalog.PlantDelete)]
        public async Task<IActionResult> DeletePlant(int id, CancellationToken cancellationToken = default)
        {
            await _plantService.DeletePlantAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
