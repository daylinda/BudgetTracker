//using Android.Telephony;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TrackerApp.MAUI.Services;
using TrackerApp.Shared.Model;

//using static Java.Util.Jar.Attributes;

namespace TrackerApp.MAUI.ViewModels;

public partial class AddBudgetViewModel : BaseViewModel
{
    private readonly ApiBudgetService _budgetService;
    private readonly string _userId;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private decimal _limitAmount;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError;

    public AddBudgetViewModel(ApiBudgetService budgetService)
    {
        _budgetService = budgetService;
        _userId = Preferences.Get("userId", string.Empty);
        Title = "Add Budget";
    }

    [RelayCommand]
    private async Task AddBudgetAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ErrorMessage = "Please enter a budget name.";
            HasError = true;
            return;
        }

        if (LimitAmount <= 0)
        {
            ErrorMessage = "Please enter a valid amount.";
            HasError = true;
            return;
        }

        IsBusy = true;
        HasError = false;

        try
        {
            var budget = new Budget
            {
                Name = Name,
                Category = Category,
                LimitAmount = LimitAmount,
                SpentAmount = 0
            };

            var result = await _budgetService.SetBudgetAsync(_userId, budget);

            // debug — check what came back
            System.Diagnostics.Debug.WriteLine($"Result: {result?.BudgetId ?? "NULL"}");
            System.Diagnostics.Debug.WriteLine($"UserId: {_userId}");


            // go back to budget list
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to add budget. Please try again.";
            HasError = true;
        }
        finally
        {
            IsBusy = false;
        }
    }
}