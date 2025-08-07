using AutoMapper;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Simpl.Expenses.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(IGenericRepository<Supplier> supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<SupplierDto> GetSupplierByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<SupplierDto>(supplier);
        }

        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync(CancellationToken cancellationToken = default)
        {
            var suppliers = await _supplierRepository.GetAll(cancellationToken).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
        }

        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto createSupplierDto, CancellationToken cancellationToken = default)
        {
            var supplier = _mapper.Map<Supplier>(createSupplierDto);
            await _supplierRepository.AddAsync(supplier, cancellationToken);
            return _mapper.Map<SupplierDto>(supplier);
        }

        public async Task UpdateSupplierAsync(int id, UpdateSupplierDto updateSupplierDto, CancellationToken cancellationToken = default)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id, cancellationToken);
            if (supplier == null) return;

            _mapper.Map(updateSupplierDto, supplier);

            await _supplierRepository.UpdateAsync(supplier, cancellationToken);
        }

        public async Task DeleteSupplierAsync(int id, CancellationToken cancellationToken = default)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id, cancellationToken);
            if (supplier != null)
            {
                await _supplierRepository.RemoveAsync(supplier, cancellationToken);
            }
        }
    }
}
