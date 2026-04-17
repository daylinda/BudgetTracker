using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Options;
using TrackerApp.API.Config;
using TrackerApp.API.Model;
using TrackerApp.API.Repositories.Interfaces;

namespace TrackerApp.API.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly FirebaseClient _firebase;

    public BudgetRepository(IOptions<FirebaseSettings> options)
    {
        _firebase = new FirebaseClient(options.Value.DatabaseUrl);
    }

    public async Task<List<Budget>> GetAllAsync(string userId)
    {
        var budgets = await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .OnceAsync<Budget>();

        return budgets.Select(b => b.Object).ToList();
    }

    public async Task<Budget?> GetByIdAsync(string userId, string budgetId)
    {
        var budget = await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .Child(budgetId)
            .OnceSingleAsync<Budget>();

        return budget;
    }

    public async Task<Budget> CreateAsync(string userId, Budget budget)
    {
        budget.UserId = userId;
        budget.SpentAmount = 0;
        budget.CreatedAt = DateTime.UtcNow;

        var result = await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .PostAsync(budget);

        budget.BudgetId = result.Key;

        // save the generated key back into firebase
        await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .Child(budget.BudgetId)
            .PutAsync(budget);

        return budget;
    }

    public async Task<Budget?> ResetAsync(string userId, string budgetId)
    {
        var budget = await GetByIdAsync(userId, budgetId);
        if (budget is null) return null;

        // zero out spent, keep the limit
        budget.SpentAmount = 0;

        await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .Child(budgetId)
            .PutAsync(budget);

        return budget;
    }

    public async Task<bool> DeleteAsync(string userId, string budgetId)
    {
        var budget = await GetByIdAsync(userId, budgetId);
        if (budget is null) return false;

        await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .Child(budgetId)
            .DeleteAsync();

        return true;
    }
}