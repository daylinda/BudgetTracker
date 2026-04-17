using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Options;
using TrackerApp.API.Config;
using TrackerApp.API.Model;
using TrackerApp.API.Repositories.Interfaces;

namespace TrackerApp.API.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly FirebaseClient _firebase;

    public NotificationRepository(IOptions<FirebaseSettings> options)
    {
        _firebase = new FirebaseClient(options.Value.DatabaseUrl);
    }

    public async Task AddAsync(Notification notification)
    {
        await _firebase
            .Child("notifications")
            .PostAsync(notification);
    }

    public async Task<List<Notification>> GetAllAsync()
    {
        var notifications = await _firebase
            .Child("notifications")
            .OnceAsync<Notification>();

        return notifications.Select(item => item.Object).ToList();
    }
}