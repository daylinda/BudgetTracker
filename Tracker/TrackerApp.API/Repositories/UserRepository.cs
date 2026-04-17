using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Options;
using TrackerApp.API.Config;
using TrackerApp.API.Model;
using TrackerApp.API.Repositories.Interfaces;

namespace TrackerApp.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FirebaseClient _firebase;

    public UserRepository(IOptions<FirebaseSettings> options)
    {
        _firebase = new FirebaseClient(options.Value.DatabaseUrl);
    }

    public async Task<List<User>> GetAllAsync()
    {
        var users = await _firebase
            .Child("users")
            .OnceAsync<User>();

        return users.Select(u => u.Object).ToList();
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        var users = await _firebase
            .Child("users")
            .OnceAsync<User>();

        return users
            .Select(u => u.Object)
            .FirstOrDefault(u => u.UserId == userId);
    }

    public async Task<User> CreateAsync(User user)
    {

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        await _firebase
            .Child("users")
            .Child(user.UserId.ToString())
            .PutAsync(user);

        return user;
    }

    public async Task<User?> UpdateAsync(int userId, User updated)
    {
        var existing = await GetByIdAsync(userId);
        if (existing is null) return null;

        updated.UserId = userId;

        await _firebase
            .Child("users")
            .Child(userId.ToString())
            .PutAsync(updated);

        return updated;
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var existing = await GetByIdAsync(userId);
        if (existing is null) return false;

        await _firebase
            .Child("users")
            .Child(userId.ToString())
            .DeleteAsync();

        return true;
    }
}