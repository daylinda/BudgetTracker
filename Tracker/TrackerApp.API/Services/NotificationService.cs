using TrackerApp.API.Model;
using TrackerApp.API.Repositories.Interfaces;

namespace TrackerApp.API.Services;

public class NotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task AddNotification(Notification notification) =>
        await _notificationRepository.AddAsync(notification);

    public async Task<List<Notification>> GetNotifications() =>
        await _notificationRepository.GetAllAsync();
}