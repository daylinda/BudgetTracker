using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TrackerApp.Shared.Model;

namespace TrackerApp.MAUI.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _userId = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public LoginViewModel()
    {
        Title = "Login";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(UserId))
        {
            ErrorMessage = "Please enter your User ID.";
            return;
        }

        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            // save userId locally on the device
            Preferences.Set("userId", UserId);

            // navigate to BudgetListPage
            await Shell.Current.GoToAsync("//BudgetListPage");
        }
        catch (Exception ex)
        {
            ErrorMessage = "Login failed. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}