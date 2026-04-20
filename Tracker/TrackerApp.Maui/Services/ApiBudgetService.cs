using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using TrackerApp.MAUI.Config;
using TrackerApp.Shared.Model;


namespace TrackerApp.MAUI.Services;

public class ApiBudgetService
{
    private readonly HttpClient _httpClient;

    public ApiBudgetService(HttpClient httpClient, IOptions<ApiSettings> options)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
    }

    public async Task<List<Budget>> GetBudgetsAsync(string userId)
    {
        var result = await _httpClient
            .GetFromJsonAsync<List<Budget>>($"api/{userId}/budget");
        return result ?? new List<Budget>();
    }

    public async Task<bool> DeleteBudgetAsync(string userId, string budgetId)
    {
        var response = await _httpClient
            .DeleteAsync($"api/{userId}/budget/{budgetId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ResetBudgetAsync(string userId, string budgetId)
    {
        var response = await _httpClient
            .PatchAsync($"api/{userId}/budget/{budgetId}/reset", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<Budget?> SetBudgetAsync(string userId, Budget budget)
    {
        var response = await _httpClient
            .PostAsJsonAsync($"api/{userId}/budget", budget);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<Budget>()
            : null;
    }

    public async Task<List<Transaction>> GetTransactionsAsync(string userId, string budgetId)
    {
        var result = await _httpClient
            .GetFromJsonAsync<List<Transaction>>(
                $"api/{userId}/budget/{budgetId}/transactions");
        return result ?? new List<Transaction>();
    }

    public async Task<bool> DeleteTransactionAsync(string userId, string budgetId, string transactionId)
    {
        var response = await _httpClient
            .DeleteAsync($"api/{userId}/budget/{budgetId}/transactions/{transactionId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<Transaction?> AddTransactionAsync(
     string userId, string budgetId, Transaction transaction)
    {
        var response = await _httpClient
            .PostAsJsonAsync(
                $"api/{userId}/budget/{budgetId}/transactions", transaction);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<Transaction>()
            : null;
    }
}