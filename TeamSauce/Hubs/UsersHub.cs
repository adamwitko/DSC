using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TeamSauce.Services;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Hubs
{
    [HubName("usersHub")]
    public class UsersHub : Hub
    {
        private readonly IUserService _userService;

        public UsersHub()
            : this(new UserService())
        {
        }

        public UsersHub(IUserService userService)
        {
            _userService = userService;
        }

        public void LogIn(string username, string password)
        {
            _userService.CreateUser(Context.ConnectionId, username, password);
        }
    }
}