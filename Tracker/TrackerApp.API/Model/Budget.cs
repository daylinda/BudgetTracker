namespace TrackerApp.API.Model;

public class Budget
{
    public string? BudgetId { get; set; }
    public string? UserId { get; set; }
    public string? Name { get; set; }
    public decimal LimitAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public string? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}