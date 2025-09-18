using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.ViewModel
{
    public partial class MainViewModel: ObservableObject
    {
        // This property holds the instance of the child ViewModel
        [ObservableProperty]
        private NotificationViewModel _notifications;

        // We get the child ViewModel via dependency injection
        public MainViewModel(NotificationViewModel notificationsViewModel)
        {
            _notifications = notificationsViewModel;
        }
    }
}
