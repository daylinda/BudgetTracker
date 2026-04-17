using TrackerApp.API.Model;
using TrackerApp.API.Repositories.Interfaces;

namespace TrackerApp.API.Services;

public class BudgetService
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ITransactionRepository _transactionRepository;

    public BudgetService(IBudgetRepository budgetRepository, ITransactionRepository transactionRepository)
    {
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<List<Budget>> GetAllBudgets(string userId) =>
        await _budgetRepository.GetAllAsync(userId);

    public async Task<Budget?> GetBudget(string userId, string budgetId) =>
        await _budgetRepository.GetByIdAsync(userId, budgetId);

    public async Task<Budget> SetBudget(string userId, Budget budget) =>
        await _budgetRepository.CreateAsync(userId, budget);

    public async Task<Budget?> ResetBudget(string userId, string budgetId) =>
        await _budgetRepository.ResetAsync(userId, budgetId);

    public async Task<bool> DeleteBudget(string userId, string budgetId) =>
        await _budgetRepository.DeleteAsync(userId, budgetId);

    // transactions
    public async Task<List<Transaction>> GetTransactions(string userId, string budgetId) =>
        await _transactionRepository.GetAllAsync(userId, budgetId);

    public async Task<Transaction> AddTransaction(string userId, string budgetId, Transaction transaction)
    {
        var result = await _transactionRepository.AddAsync(userId, budgetId, transaction);

        // update SpentAmount on the budget
        var budget = await _budgetRepository.GetByIdAsync(userId, budgetId);
        if (budget is not null)
        {
            budget.SpentAmount += transaction.Type == "out" ? transaction.Amount : -transaction.Amount;
            await _budgetRepository.CreateAsync(userId, budget);
        }

        return result;
    }

    public async Task<Transaction?> AdjustTransaction(string userId, string budgetId, string transactionId, decimal amount) =>
        await _transactionRepository.AdjustAmountAsync(userId, budgetId, transactionId, amount);

    public async Task<bool> DeleteTransaction(string userId, string budgetId, string transactionId) =>
        await _transactionRepository.DeleteAsync(userId, budgetId, transactionId);
}