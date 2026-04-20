namespace TrackerApp.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Views.AddBudgetPage), typeof(Views.AddBudgetPage));
        Routing.RegisterRoute(nameof(Views.BudgetDetailPage), typeof(Views.BudgetDetailPage));
        Routing.RegisterRoute(nameof(Views.AddTransactionPage), typeof(Views.AddTransactionPage));
    }
}