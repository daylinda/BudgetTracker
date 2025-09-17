using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Model
{
    public class Notification
    {
        public string Id { get; set; }
        public string? UserId { get; set; }   // Firebase UID
        public string? AppName { get; set; }  // e.g. WhatsApp, Gmail
        public string? Title { get; set; }
        public string? Body { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}
