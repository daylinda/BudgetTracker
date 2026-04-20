
using TrackerApp.API.Repositories.Interfaces;
using TrackerApp.Shared.Model;

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

    public async Task<User?> GetUserByUserId(string userId) =>
        await _userRepository.GetByIdAsync(userId);

    public async Task<User> CreateUser(User user) =>
        await _userRepository.CreateAsync(user);

    public async Task<User?> UpdateUser(string userId, User user) =>
        await _userRepository.UpdateAsync(userId, user);

    public async Task<bool> DeleteUser(string userId) =>
        await _userRepository.DeleteAsync(userId);
}