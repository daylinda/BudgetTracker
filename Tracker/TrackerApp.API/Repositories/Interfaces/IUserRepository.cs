using TrackerApp.API.Model;

namespace TrackerApp.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int userId);
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(int userId, User user);
    Task<bool> DeleteAsync(int userId);
}