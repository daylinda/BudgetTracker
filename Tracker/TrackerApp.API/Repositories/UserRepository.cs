using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Options;
using TrackerApp.API.Config;
using TrackerApp.API.Repositories.Interfaces;

using BCrypt.Net;
using TrackerApp.Shared.Model;

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

        return users
            .Select(u => u.Object)
            .ToList();
    }

    public async Task<User?> GetByIdAsync(string userId)
    {
        var user = await _firebase
            .Child("users")
            .Child(userId)
            .OnceSingleAsync<User>();

        return user;
    }

    public async Task<User> CreateAsync(User user)
    {
        // hash password before saving
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        await _firebase
            .Child("users")
            .Child(user.UserId)
            .PutAsync(user);

        return user;
    }

    public async Task<User?> UpdateAsync(string userId, User updated)
    {
        var existing = await GetByIdAsync(userId);
        if (existing is null) return null;

        // keep existing password if no new one provided
        if (string.IsNullOrEmpty(updated.Password))
            updated.Password = existing.Password;
        else
            updated.Password = BCrypt.Net.BCrypt.HashPassword(updated.Password);

        updated.UserId = userId;

        await _firebase
            .Child("users")
            .Child(userId)
            .PutAsync(updated);

        return updated;
    }

    public async Task<bool> DeleteAsync(string userId)
    {
        var existing = await GetByIdAsync(userId);
        if (existing is null) return false;

        await _firebase
            .Child("users")
            .Child(userId)
            .DeleteAsync();

        return true;
    }
}