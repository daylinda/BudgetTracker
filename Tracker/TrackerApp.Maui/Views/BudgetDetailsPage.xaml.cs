using TrackerApp.MAUI.ViewModels;

namespace TrackerApp.MAUI.Views;

public partial class BudgetDetailPage : ContentPage
{
    private readonly BudgetDetailViewModel _viewModel;

    public BudgetDetailPage(BudgetDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadTransactionsCommand.Execute(null);
    }
}