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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.SupplierRead)]
        public async Task<IActionResult> GetSupplierById(int id, CancellationToken cancellationToken = default)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id, cancellationToken);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }

        [HttpGet]
        [Authorize(Policy = PermissionCatalog.SupplierRead)]
        public async Task<IActionResult> GetAllSuppliers(CancellationToken cancellationToken = default)
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync(cancellationToken);
            return Ok(suppliers);
        }

        [HttpPost]
        [Authorize(Policy = PermissionCatalog.SupplierCreate)]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierDto createSupplierDto, CancellationToken cancellationToken = default)
        {
            var newSupplier = await _supplierService.CreateSupplierAsync(createSupplierDto, cancellationToken);
            return CreatedAtAction(nameof(GetSupplierById), new { id = newSupplier.Id }, newSupplier);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionCatalog.SupplierUpdate)]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDto updateSupplierDto, CancellationToken cancellationToken = default)
        {
            await _supplierService.UpdateSupplierAsync(id, updateSupplierDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionCatalog.SupplierDelete)]
        public async Task<IActionResult> DeleteSupplier(int id, CancellationToken cancellationToken = default)
        {
            await _supplierService.DeleteSupplierAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
