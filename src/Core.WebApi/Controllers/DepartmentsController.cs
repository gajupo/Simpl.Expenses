using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Application.Interfaces;
using System.Threading.Tasks;

namespace Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id, CancellationToken cancellationToken = default)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id, cancellationToken);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments(CancellationToken cancellationToken = default)
        {
            var departments = await _departmentService.GetAllDepartmentsAsync(cancellationToken);
            return Ok(departments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto createDepartmentDto, CancellationToken cancellationToken = default)
        {
            var newDepartment = await _departmentService.CreateDepartmentAsync(createDepartmentDto, cancellationToken);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = newDepartment.Id }, newDepartment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto updateDepartmentDto, CancellationToken cancellationToken = default)
        {
            await _departmentService.UpdateDepartmentAsync(id, updateDepartmentDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id, CancellationToken cancellationToken = default)
        {
            await _departmentService.DeleteDepartmentAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
