using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using TeamSauce.Connections.TeamChatConnection.Data;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Connections.TeamChatConnection
{
    public class TeamChatConnection : PersistentConnection
    {
        private readonly ITeamChatMessageService _teamChatMessageService;
        private readonly Dictionary<string, string> _clients = new Dictionary<string, string>();

        public TeamChatConnection()
            : this(ServiceFactory.GetTeamChatMessageService())
        {
        }

        public TeamChatConnection(ITeamChatMessageService teamChatMessageService)
        {
            _teamChatMessageService = teamChatMessageService;
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            _clients.Add(connectionId, string.Empty);
            
            var messages = _teamChatMessageService.GetTeamMessages(connectionId);

            return Connection.Broadcast(messages);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            var chatData = JsonConvert.DeserializeObject<TeamChatPost>(data);
            _clients[connectionId] = chatData.Name;

            var teamChatMessage = TeamChatMessage.FromResponse(chatData);
            _teamChatMessageService.AddMessage(teamChatMessage);

            return Connection.Broadcast(new List<TeamChatMessage> { teamChatMessage });
        }

        protected override Task OnDisconnected(IRequest request, string connectionId)
        {
            var name = _clients[connectionId];
            var chatData = new TeamChatMessage("Server", string.Format("{0} has disconnected.", name));
            
            _clients.Remove(connectionId);
            
            return Connection.Broadcast(chatData);
        }
    }
}