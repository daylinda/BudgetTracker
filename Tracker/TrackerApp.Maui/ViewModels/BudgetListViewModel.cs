//using Android.Telephony;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TrackerApp.MAUI.Services;
using TrackerApp.Shared.Model;


namespace TrackerApp.MAUI.ViewModels;

public partial class BudgetListViewModel : BaseViewModel
{
    private readonly ApiBudgetService _budgetService;
    private readonly string _userId;

    [ObservableProperty]
    private ObservableCollection<Budget> _budgets = new();

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError;

    public BudgetListViewModel(ApiBudgetService budgetService)
    {
        _budgetService = budgetService;
        _userId = Preferences.Get("userId", string.Empty);
        Title = "My Budgets";
    }

    [RelayCommand]
    private async Task LoadBudgetsAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        HasError = false;

        try
        {
            var budgets = await _budgetService.GetBudgetsAsync(_userId);
            Budgets.Clear();
            foreach (var b in budgets)
                Budgets.Add(b);
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to load budgets.";
            HasError = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ResetBudgetAsync(string budgetId)
    {
        try
        {
            await _budgetService.ResetBudgetAsync(_userId, budgetId);
            await LoadBudgetsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to reset budget.";
            HasError = true;
        }
    }

    [RelayCommand]
    private async Task DeleteBudgetAsync(string budgetId)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Delete Budget",
            "Are you sure you want to delete this budget?",
            "Yes", "No");

        if (!confirm) return;

        try
        {
            await _budgetService.DeleteBudgetAsync(_userId, budgetId);
            await LoadBudgetsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to delete budget.";
            HasError = true;
        }
    }

    [RelayCommand]
    private async Task GoToAddBudgetAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.AddBudgetPage));
        await LoadBudgetsAsync();
        // coming soon
        //await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task GoToBudgetDetailAsync(Budget budget)
    {
        var navigationParameter = new Dictionary<string, object>
    {
        { "Budget", budget }
    };
        await Shell.Current.GoToAsync(nameof(Views.BudgetDetailPage), navigationParameter);
    }
}