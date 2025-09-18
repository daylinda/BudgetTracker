using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Model;

namespace Tracker.IServices
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationsAsync();
        Task PostNotificationAsync(Notification notification);

    }
}
