using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TeamSauce.Models;
using TeamSauce.Models.Factories;
using TeamSauce.Services;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Hubs
{
    [HubName("sponsorHub")]
    public class SponsorChatHub : Hub
    {
        private readonly IUserService _userService;
        private readonly ISponsorMessageService _sponsorMessageService;
        private readonly ISponsorMessageModelFactory _modelFactory;

        private readonly IHubConnectionContext _clientContext = 
            GlobalHost.ConnectionManager.GetHubContext<SponsorChatHub>().Clients;

        public SponsorChatHub()
            : this(new UserService(), new SponsorMessageService(), new SponsorMessageModelFactory())
        {
        }

        public SponsorChatHub(IUserService userService, ISponsorMessageService sponsorMessageService, ISponsorMessageModelFactory modelFactory)
        {
            _userService = userService;
            _sponsorMessageService = sponsorMessageService;
            _modelFactory = modelFactory;
        }

        public void MessageReceived(string message)
        {
            var sender = _userService.GetUser(Context.ConnectionId);
            
            var sponsorMessageModel = _modelFactory.Create(message, sender);
            
            _sponsorMessageService.PersistMessage(sponsorMessageModel);

            switch (sender.UserType)
            {
                case "User":
                    _clientContext.All.UserMessage(sponsorMessageModel);
                    break;
                case "Sponsor":
                    _clientContext.All.SponsorMessage(sponsorMessageModel);
                    break;
            }
        }

        public void GetMessages()
        {
            var messages = _sponsorMessageService.GetMessages();
            _clientContext.Client(Context.ConnectionId).MessagesLoaded(messages);
        }
    }
}