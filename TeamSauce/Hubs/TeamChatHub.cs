using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TeamSauce.Models.Factories;
using TeamSauce.Services;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Hubs
{
    [HubName("teamChatHub")]
    public class TeamChatHub : Hub
    {
        private readonly IUserService _userService;
        private readonly ITeamMessageModelFactory _modelFactory;

        private readonly IHubConnectionContext _clientContext =
            GlobalHost.ConnectionManager.GetHubContext<SponsorChatHub>().Clients;

        public TeamChatHub()
            : this(new UserService(), new TeamMessageModelFactory())
        {
        }

        public TeamChatHub(IUserService userService, ITeamMessageModelFactory modelFactory)
        {
            _userService = userService;
            _modelFactory = modelFactory;
        }

        public void MessageReceived(string message)
        {
            var sender = _userService.GetUser(Context.ConnectionId);
            var model = _modelFactory.Create(message, sender.Name);

            _clientContext.All.TeamMessageReceived(model);
        }
    }
}