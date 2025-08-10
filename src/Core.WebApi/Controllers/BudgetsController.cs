using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Simpl.Expenses.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetDto>> GetBudgetById(int id, CancellationToken cancellationToken)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id, cancellationToken);
            if (budget == null)
            {
                return NotFound();
            }
            return Ok(budget);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetDto>>> GetAllBudgets(CancellationToken cancellationToken)
        {
            var budgets = await _budgetService.GetAllBudgetsAsync(cancellationToken);
            return Ok(budgets);
        }

        [HttpPost]
        public async Task<ActionResult<BudgetDto>> CreateBudget([FromBody] CreateBudgetDto createBudgetDto, CancellationToken cancellationToken)
        {
            var createdBudget = await _budgetService.CreateBudgetAsync(createBudgetDto, cancellationToken);
            return CreatedAtAction(nameof(GetBudgetById), new { id = createdBudget.Id }, createdBudget);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] UpdateBudgetDto updateBudgetDto, CancellationToken cancellationToken)
        {
            await _budgetService.UpdateBudgetAsync(id, updateBudgetDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id, CancellationToken cancellationToken)
        {
            await _budgetService.DeleteBudgetAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
