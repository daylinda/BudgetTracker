using TrackerApp.API.Model;

namespace TrackerApp.API.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetAllAsync(string userId, string budgetId);
    Task<Transaction?> GetByIdAsync(string userId, string budgetId, string transactionId);
    Task<Transaction> AddAsync(string userId, string budgetId, Transaction transaction);
    Task<Transaction?> AdjustAmountAsync(string userId, string budgetId, string transactionId, decimal amount);
    Task<bool> DeleteAsync(string userId, string budgetId, string transactionId);
}