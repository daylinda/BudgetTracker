using TrackerApp.API.Model;

namespace TrackerApp.API.Repositories.Interfaces;

public interface IBudgetRepository
{
    Task<List<Budget>> GetAllAsync(string userId);
    Task<Budget?> GetByIdAsync(string userId, string budgetId);
    Task<Budget> CreateAsync(string userId, Budget budget);
    Task<Budget?> ResetAsync(string userId, string budgetId);
    Task<bool> DeleteAsync(string userId, string budgetId);
}