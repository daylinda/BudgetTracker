using TrackerApp.Shared.Model;


namespace TrackerApp.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string userId);
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(string userId, User updated);
    Task<bool> DeleteAsync(string userId);
}