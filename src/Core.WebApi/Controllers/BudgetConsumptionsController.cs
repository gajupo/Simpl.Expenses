using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<BudgetConsumptionDto>>> GetAllBudgetConsumptions(CancellationToken cancellationToken)
        {
            var budgetConsumptions = await _budgetConsumptionService.GetAllBudgetConsumptionsAsync(cancellationToken);
            return Ok(budgetConsumptions);
        }

        [HttpGet("range")]
        public async Task<ActionResult<IEnumerable<BudgetConsumptionDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
        {
            return Ok(await _budgetConsumptionService.GetBudgetConsumptionsByDateRangeAsync(startDate, endDate, cancellationToken));
        }

        [HttpGet("cost-center")]
        public async Task<ActionResult<IEnumerable<BudgetConsumptionDto>>> GetByDateRangeAndCostCenter([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int costCenterId, CancellationToken cancellationToken)
        {
            return Ok(await _budgetConsumptionService.GetBudgetConsumptionsByDateRangeAndCostCenterAsync(startDate, endDate, costCenterId, cancellationToken));
        }

        [HttpGet("project")]
        public async Task<ActionResult<IEnumerable<BudgetConsumptionDto>>> GetByDateRangeAndAccountProject([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int accountProjectId, CancellationToken cancellationToken)
        {
            return Ok(await _budgetConsumptionService.GetBudgetConsumptionsByDateRangeAndAccountProjectAsync(startDate, endDate, accountProjectId, cancellationToken));
        }
    }
}
