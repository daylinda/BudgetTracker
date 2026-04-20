using TrackerApp.MAUI.ViewModels;

namespace TrackerApp.MAUI.Views;

public partial class BudgetListPage : ContentPage
{
    private readonly BudgetListViewModel _viewModel;

    public BudgetListPage(BudgetListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadBudgetsCommand.Execute(null);
    }
}