using TrackerApp.MAUI.ViewModels;

namespace TrackerApp.MAUI.Views;

public partial class AddTransactionPage : ContentPage
{
	public AddTransactionPage(AddTransactionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}