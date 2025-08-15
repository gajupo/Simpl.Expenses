using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Simpl.Expenses.Domain.Constants;

namespace Simpl.Expenses.WebAPI.Controllers
{
    [Route("api/budget-consumptions")]
    [ApiController]
    public class BudgetConsumptionsController : ControllerBase
    {
        private readonly IBudgetConsumptionService _budgetConsumptionService;

        public BudgetConsumptionsController(IBudgetConsumptionService budgetConsumptionService)
        {
            _budgetConsumptionService = budgetConsumptionService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.BudgetConsumptionRead)]
        public async Task<ActionResult<BudgetConsumptionDto>> GetBudgetConsumptionById(int id, CancellationToken cancellationToken)
        {
            var budgetConsumption = await _budgetConsumptionService.GetBudgetConsumptionByIdAsync(id, cancellationToken);
            if (budgetConsumption == null)
            {
                return NotFound();
            }
            return Ok(budgetConsumption);
        }

        [HttpGet]
        [Authorize(Policy = PermissionCatalog.BudgetConsumptionRead)]
        public async Task<ActionResult<IEnumerable<BudgetConsumptionDto>>> GetAllBudgetConsumptions(CancellationToken cancellationToken)
        {
            var budgetConsumptions = await _budgetConsumptionService.GetAllBudgetConsumptionsAsync(cancellationToken);
            return Ok(budgetConsumptions);
        }

        [HttpGet("range")]
        [Authorize(Policy = PermissionCatalog.BudgetConsumptionRead)]
        public async Task<ActionResult<IEnumerable<BudgetConsumptionDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
        {
            return Ok(await _budgetConsumptionService.GetBudgetConsumptionsByDateRangeAsync(startDate, endDate, cancellationToken));
        }

        [HttpGet("cost-center")]
        [Authorize(Policy = PermissionCatalog.BudgetConsumptionRead)]
        public async Task<ActionResult<IEnumerable<BudgetConsumptionDto>>> GetByDateRangeAndCostCenter([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int costCenterId, CancellationToken cancellationToken)
        {
            return Ok(await _budgetConsumptionService.GetBudgetConsumptionsByDateRangeAndCostCenterAsync(startDate, endDate, costCenterId, cancellationToken));
        }

        [HttpGet("project")]
        [Authorize(Policy = PermissionCatalog.BudgetConsumptionRead)]
        public async Task<ActionResult<IEnumerable<BudgetConsumptionDto>>> GetByDateRangeAndAccountProject([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int accountProjectId, CancellationToken cancellationToken)
        {
            return Ok(await _budgetConsumptionService.GetBudgetConsumptionsByDateRangeAndAccountProjectAsync(startDate, endDate, accountProjectId, cancellationToken));
        }

        [HttpGet("percentage-consumptions/{centerCostId}")]
        [Authorize(Policy = PermissionCatalog.BudgetConsumptionRead)]
        public async Task<ActionResult<IEnumerable<BudgetConsumptionDto>>> GetTotalBudgetConsumptionsByCenterCost(int centerCostId, CancellationToken cancellationToken)
        {
            return Ok(await _budgetConsumptionService.GetPercentageBudgetConsumptionsByCenterCostAsync(centerCostId, cancellationToken));
        }
    }
}
