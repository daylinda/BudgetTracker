
using CommunityToolkit.Mvvm.ComponentModel;  // ObservableProperty, ObservableObject
using CommunityToolkit.Mvvm.Input;           // RelayCommand

using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private bool _isBusy;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Notification> _notifications = new();

        public NotificationViewModel(INotificationService notificationService) {
            WelcomeText = "Hi! Hope you are well";
            _notificationService = notificationService;


        }

        [RelayCommand]
        private async Task GetNotificationsAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;
                var results = await _notificationService.GetNotificationsAsync();
                Notifications.Clear();
                foreach (var note in results)
                    Notifications.Add(note);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load. Please try again.";
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally { IsBusy = false; }
        }

        private async Task<List<Notification>> getNotificationsAsync()
        {
           var notification = await _notificationService.GetNotificationsAsync();
            return notification;
        }
    }
}
