using Simpl.Expenses.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<SupplierDto> GetSupplierByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync(CancellationToken cancellationToken = default);
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto createSupplierDto, CancellationToken cancellationToken = default);
        Task UpdateSupplierAsync(int id, UpdateSupplierDto updateSupplierDto, CancellationToken cancellationToken = default);
        Task DeleteSupplierAsync(int id, CancellationToken cancellationToken = default);
    }
}
