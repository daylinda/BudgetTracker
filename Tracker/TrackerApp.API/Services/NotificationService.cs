using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Options;
using TrackerApp.API.Config;
using TrackerApp.API.Model;



namespace TrackerApp.API.Services
{
    
    public class NotificationService
    {
        private readonly FirebaseClient _firebase;

        public NotificationService(IOptions<FirebaseSettings> options)
        {
            var option = options.Value;
            _firebase = new FirebaseClient(option.DatabaseUrl);
        }

        public async Task AddNotification(Notification notification)
        {
            await _firebase
                .Child("notifications")
                .PostAsync(notification);
        }

        public async Task<List<Notification>> GetNotifications()
        {
            var notifications = await _firebase
                .Child("notifications")
                .OnceAsync<Notification>();

            return notifications.Select(item => item.Object).ToList();
        }
    }

}
