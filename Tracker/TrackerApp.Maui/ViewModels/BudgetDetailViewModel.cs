//using Android.Telephony;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TrackerApp.MAUI.Services;
using TrackerApp.Shared.Model;


namespace TrackerApp.MAUI.ViewModels;

[QueryProperty(nameof(Budget), "Budget")]
public partial class BudgetDetailViewModel : BaseViewModel
{
    private readonly ApiBudgetService _budgetService;
    private readonly string _userId;

    [ObservableProperty]
    private Budget _budget = new();

    [ObservableProperty]
    private ObservableCollection<Transaction> _transactions = new();

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError;

    public BudgetDetailViewModel(ApiBudgetService budgetService)
    {
        _budgetService = budgetService;
        _userId = Preferences.Get("userId", string.Empty);
        Title = "Budget Details";
    }

    [RelayCommand]
    private async Task LoadTransactionsAsync()
    {
        if (IsBusy || Budget?.BudgetId is null) return;
        IsBusy = true;
        HasError = false;

        try
        {
            var transactions = await _budgetService
                .GetTransactionsAsync(_userId, Budget.BudgetId);
            Transactions.Clear();
            foreach (var t in transactions)
                Transactions.Add(t);
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to load transactions.";
            HasError = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteTransactionAsync(string transactionId)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Delete Transaction",
            "Are you sure?",
            "Yes", "No");

        if (!confirm) return;

        try
        {
            await _budgetService.DeleteTransactionAsync(
                _userId, Budget.BudgetId!, transactionId);
            await LoadTransactionsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to delete transaction.";
            HasError = true;
        }
    }

    [RelayCommand]
    private async Task GoToAddTransactionAsync()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "Budget", Budget }
        };
        await Shell.Current.GoToAsync(
            nameof(Views.AddTransactionPage), navigationParameter);
    }
}