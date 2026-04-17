namespace TrackerApp.Shared.Model;

public class Transaction
{
    public string? TransactionId { get; set; }
    public string? UserId { get; set; }
    public string? BudgetId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; } // "in" or "out"
    public DateTime Date { get; set; } = DateTime.UtcNow;
}