using TrackerApp.MAUI.ViewModels;

namespace TrackerApp.MAUI.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}