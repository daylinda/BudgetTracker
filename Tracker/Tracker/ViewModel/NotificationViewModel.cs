using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.IServices;
using Tracker.Model;

namespace Tracker.ViewModel
{
    public partial class NotificationViewModel: ObservableObject
    {

        private INotificationService _notificationService;

        [ObservableProperty]
        private string _welcomeText;

        [ObservableProperty]
        private List<Notification> _notifications;

        public NotificationViewModel(INotificationService notificationService) {
            WelcomeText = "Hi! Hope you are well";
            _notificationService = notificationService;


        }

        [RelayCommand]
        private void GetNotifications()
        {
            Debug.WriteLine("here");
            Notifications = getNotificationsAsync().Result.ToList();
            
                

            foreach(Notification note in Notifications)
            {
                Console.WriteLine("here");
            }

        }

        private async Task<List<Notification>> getNotificationsAsync()
        {
           var notification = await _notificationService.GetNotificationsAsync();
            return notification;
        }
    }
}
