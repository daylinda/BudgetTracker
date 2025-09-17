namespace TrackerApp.API.Model
{
    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }   // Firebase UID
        public string? AppName { get; set; }  // e.g. WhatsApp, Gmail
        public string? Title { get; set; }
        public string? Body { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }

}
    