
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TrackerApp.MAUI.Services;
using TrackerApp.Shared.Model;


namespace TrackerApp.MAUI.ViewModels;

[QueryProperty(nameof(Budget), "Budget")]
public partial class AddTransactionViewModel : BaseViewModel
{
    private readonly ApiBudgetService _budgetService;
    private readonly string _userId;

    [ObservableProperty]
    private Budget _budget = new();

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private decimal _amount;

    [ObservableProperty]
    private bool _isMoneyIn = true;

    [ObservableProperty]
    private bool _isMoneyOut;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError;

    public AddTransactionViewModel(ApiBudgetService budgetService)
    {
        _budgetService = budgetService;
        _userId = Preferences.Get("userId", string.Empty);
        Title = "Add Transaction";
    }

    [RelayCommand]
    private async Task AddTransactionAsync()
    {
        if (string.IsNullOrWhiteSpace(Description))
        {
            ErrorMessage = "Please enter a description.";
            HasError = true;
            return;
        }

        if (Amount <= 0)
        {
            ErrorMessage = "Please enter a valid amount.";
            HasError = true;
            return;
        }

        IsBusy = true;
        HasError = false;

        try
        {
            var transaction = new Transaction
            {
                Description = Description,
                Amount = Amount,
                Type = IsMoneyIn ? "in" : "out",
                Date = DateTime.UtcNow
            };

            await _budgetService.AddTransactionAsync(
                _userId, Budget.BudgetId!, transaction);

            // go back to budget detail
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to add transaction. Please try again.";
            HasError = true;
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}