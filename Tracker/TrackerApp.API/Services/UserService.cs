using TrackerApp.API.Model;
using TrackerApp.API.Repositories.Interfaces;

namespace TrackerApp.API.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAllUsers() =>
        await _userRepository.GetAllAsync();

    public async Task<User?> GetUserByUserId(int userId) =>
        await _userRepository.GetByIdAsync(userId);

    public async Task<User> CreateUser(User user) =>
        await _userRepository.CreateAsync(user);

    public async Task<User?> UpdateUser(int userId, User user) =>
        await _userRepository.UpdateAsync(userId, user);

    public async Task<bool> DeleteUser(int userId) =>
        await _userRepository.DeleteAsync(userId);
}