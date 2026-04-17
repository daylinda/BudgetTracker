using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Options;
using TrackerApp.API.Config;
using TrackerApp.API.Model;
using TrackerApp.API.Repositories.Interfaces;

namespace TrackerApp.API.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly FirebaseClient _firebase;

    public TransactionRepository(IOptions<FirebaseSettings> options)
    {
        _firebase = new FirebaseClient(options.Value.DatabaseUrl);
    }

    public async Task<List<Transaction>> GetAllAsync(string userId, string budgetId)
    {
        var transactions = await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .Child(budgetId)
            .Child("transactions")
            .OnceAsync<Transaction>();

        return transactions.Select(t => t.Object).ToList();
    }

    public async Task<Transaction?> GetByIdAsync(string userId, string budgetId, string transactionId)
    {
        return await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .Child(budgetId)
            .Child("transactions")
            .Child(transactionId)
            .OnceSingleAsync<Transaction>();
    }

    public async Task<Transaction> AddAsync(string userId, string budgetId, Transaction transaction)
    {
        transaction.UserId = userId;
        transaction.BudgetId = budgetId;
        transaction.Date = DateTime.UtcNow;

        var result = await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .Child(budgetId)
            .Child("transactions")
            .PostAsync(transaction);

        transaction.TransactionId = result.Key;

        await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .Child(budgetId)
            .Child("transactions")
            .Child(transaction.TransactionId)
            .PutAsync(transaction);

        return transaction;
    }

    public async Task<Transaction?> AdjustAmountAsync(string userId, string budgetId, string transactionId, decimal amount)
    {
        var transaction = await GetByIdAsync(userId, budgetId, transactionId);
        if (transaction is null) return null;

        // positive amount = add, negative amount = subtract
        transaction.Amount += amount;

        await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .Child(budgetId)
            .Child("transactions")
            .Child(transactionId)
            .PutAsync(transaction);

        return transaction;
    }

    public async Task<bool> DeleteAsync(string userId, string budgetId, string transactionId)
    {
        var transaction = await GetByIdAsync(userId, budgetId, transactionId);
        if (transaction is null) return false;

        await _firebase
            .Child("users")
            .Child(userId)
            .Child("budgets")
            .Child(budgetId)
            .Child("transactions")
            .Child(transactionId)
            .DeleteAsync();

        return true;
    }
}