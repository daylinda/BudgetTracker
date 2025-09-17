using Firebase.Database;
using Microsoft.Extensions.Options;
using TrackerApp.API.Config;

namespace TrackerApp.API.Services
{
    public class UserService
    {
        private readonly FirebaseClient _firebase;

        public UserService(IOptions<FirebaseSettings> options) { 
            var option = options.Value;
            _firebase = new FirebaseClient(option.DatabaseUrl);
        }


    }
}
