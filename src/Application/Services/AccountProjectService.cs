using AutoMapper;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Simpl.Expenses.Application.Services
{
    public class AccountProjectService : IAccountProjectService
    {
        private readonly IGenericRepository<AccountProject> _accountProjectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountProjectService(IGenericRepository<AccountProject> accountProjectRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _accountProjectRepository = accountProjectRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AccountProjectDto> GetAccountProjectByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var accountProject = await _accountProjectRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<AccountProjectDto>(accountProject);
        }

        public async Task<IEnumerable<AccountProjectDto>> GetAllAccountProjectsAsync(CancellationToken cancellationToken = default)
        {
            var accountProjects = await _accountProjectRepository.GetAll(cancellationToken).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<AccountProjectDto>>(accountProjects);
        }

        public async Task<AccountProjectDto> CreateAccountProjectAsync(CreateAccountProjectDto createAccountProjectDto, CancellationToken cancellationToken = default)
        {
            var accountProject = _mapper.Map<AccountProject>(createAccountProjectDto);
            await _accountProjectRepository.AddAsync(accountProject, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<AccountProjectDto>(accountProject);
        }

        public async Task UpdateAccountProjectAsync(int id, UpdateAccountProjectDto updateAccountProjectDto, CancellationToken cancellationToken = default)
        {
            var accountProject = await _accountProjectRepository.GetByIdAsync(id, cancellationToken);
            if (accountProject == null) return;

            _mapper.Map(updateAccountProjectDto, accountProject);

            await _accountProjectRepository.UpdateAsync(accountProject, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAccountProjectAsync(int id, CancellationToken cancellationToken = default)
        {
            var accountProject = await _accountProjectRepository.GetByIdAsync(id, cancellationToken);
            if (accountProject != null)
            {
                await _accountProjectRepository.RemoveAsync(accountProject, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
