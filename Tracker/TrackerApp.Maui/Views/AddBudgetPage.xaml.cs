
using TrackerApp.MAUI.ViewModels;

namespace TrackerApp.MAUI.Views;

public partial class AddBudgetPage : ContentPage
{
    public AddBudgetPage(AddBudgetViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}