using TrackerApp.API.Model;

namespace TrackerApp.API.Repositories.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task<List<Notification>> GetAllAsync();
}