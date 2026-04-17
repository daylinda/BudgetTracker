using Microsoft.AspNetCore.Mvc;
using TrackerApp.API.Model;
using TrackerApp.API.Services;

namespace TrackerApp.API.Controllers;

[Route("api/{userId}/[controller]")]
[ApiController]
public class BudgetController : ControllerBase
{
    private readonly BudgetService _budgetService;

    public BudgetController(BudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    // GET api/{userId}/budget
    [HttpGet]
    public async Task<IActionResult> GetAll(string userId)
    {
        var budgets = await _budgetService.GetAllBudgets(userId);
        return Ok(budgets);
    }

    // GET api/{userId}/budget/{budgetId}
    [HttpGet("{budgetId}")]
    public async Task<IActionResult> GetById(string userId, string budgetId)
    {
        var budget = await _budgetService.GetBudget(userId, budgetId);
        return budget is null ? NotFound($"Budget {budgetId} not found.") : Ok(budget);
    }

    // POST api/{userId}/budget
    [HttpPost]
    public async Task<IActionResult> SetBudget(string userId, Budget budget)
    {
        if (budget is null)
            return BadRequest("Budget cannot be null.");

        var created = await _budgetService.SetBudget(userId, budget);
        return CreatedAtAction(nameof(GetById), new { userId, budgetId = created.BudgetId }, created);
    }

    // PATCH api/{userId}/budget/{budgetId}/reset
    [HttpPatch("{budgetId}/reset")]
    public async Task<IActionResult> ResetBudget(string userId, string budgetId)
    {
        var reset = await _budgetService.ResetBudget(userId, budgetId);
        return reset is null ? NotFound($"Budget {budgetId} not found.") : Ok(reset);
    }

    // DELETE api/{userId}/budget/{budgetId}
    [HttpDelete("{budgetId}")]
    public async Task<IActionResult> DeleteBudget(string userId, string budgetId)
    {
        var deleted = await _budgetService.DeleteBudget(userId, budgetId);
        return deleted ? NoContent() : NotFound($"Budget {budgetId} not found.");
    }

    // GET api/{userId}/budget/{budgetId}/transactions
    [HttpGet("{budgetId}/transactions")]
    public async Task<IActionResult> GetTransactions(string userId, string budgetId)
    {
        var transactions = await _budgetService.GetTransactions(userId, budgetId);
        return Ok(transactions);
    }

    // POST api/{userId}/budget/{budgetId}/transactions
    [HttpPost("{budgetId}/transactions")]
    public async Task<IActionResult> AddTransaction(string userId, string budgetId, Transaction transaction)
    {
        if (transaction is null)
            return BadRequest("Transaction cannot be null.");

        if (transaction.Type != "in" && transaction.Type != "out")
            return BadRequest("Transaction type must be 'in' or 'out'.");

        var created = await _budgetService.AddTransaction(userId, budgetId, transaction);
        return Ok(created);
    }

    // PATCH api/{userId}/budget/{budgetId}/transactions/{transactionId}/adjust
    [HttpPatch("{budgetId}/transactions/{transactionId}/adjust")]
    public async Task<IActionResult> AdjustTransaction(string userId, string budgetId, string transactionId, [FromQuery] decimal amount)
    {
        var adjusted = await _budgetService.AdjustTransaction(userId, budgetId, transactionId, amount);
        return adjusted is null ? NotFound($"Transaction {transactionId} not found.") : Ok(adjusted);
    }

    // DELETE api/{userId}/budget/{budgetId}/transactions/{transactionId}
    [HttpDelete("{budgetId}/transactions/{transactionId}")]
    public async Task<IActionResult> DeleteTransaction(string userId, string budgetId, string transactionId)
    {
        var deleted = await _budgetService.DeleteTransaction(userId, budgetId, transactionId);
        return deleted ? NoContent() : NotFound($"Transaction {transactionId} not found.");
    }
}