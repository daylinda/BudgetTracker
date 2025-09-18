using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tracker.Config;
using Tracker.IServices;
using Tracker.Model;

namespace Tracker.Services
{
    public class NotificationService: INotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationService(HttpClient httpClient, IOptions<Settings> options)
        {
            _httpClient = httpClient;
            var option = options.Value;
            _httpClient.BaseAddress = new Uri(option.ApiBaseUrl);
        }

        public async Task<List<Notification>> GetNotificationsAsync()
        {
            
            try
            {
                // 1. Get the raw HTTP response from the server.
                var response = await _httpClient.GetAsync("/all");

                // Optional: Checks if the request was successful (e.g., status 200 OK).
                response.EnsureSuccessStatusCode();

                // 2. Read the response content as a plain string.
                var jsonString = await response.Content.ReadAsStringAsync();

                // 3. Print the raw string to the Debug Output window.
                Debug.WriteLine("--- RAW JSON RESPONSE ---");
                Debug.WriteLine(jsonString);
                Debug.WriteLine("-------------------------");

                // Now you can try to deserialize it if you want, but the logging is the key part.
                // var notifications = JsonSerializer.Deserialize<List<Notification>>(jsonString);

                var notifications = JsonSerializer.Deserialize<List<Notification>>(jsonString,
           new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return notifications;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API call failed: {ex.Message}");
            }

            return new List<Notification>();
        }


        public async Task PostNotificationAsync(Notification notification)
        {
            await _httpClient.PostAsJsonAsync("notifications", notification);
        }
    }
}
