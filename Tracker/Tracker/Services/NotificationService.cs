using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Tracker.Model;

namespace Tracker.Services
{
    public class NotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://yourapi.com/api/");
        }

        public async Task<List<Notification>> GetNotificationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Notification>>("notifications/all");
        }


        public async Task PostNotificationAsync(Notification notification)
        {
            await _httpClient.PostAsJsonAsync("notifications", notification);
        }
    }
}
