using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using TeamSauce.Connections.TeamChatConnection.Data;
using TeamSauce.Models;

namespace TeamSauce.Connections.TeamChatConnection
{
    public class TeamChatConnection : PersistentConnection
    {
        private Dictionary<string, string> _clients = new Dictionary<string, string>();

        public void SendTime()
        {
            var data = new TeamChatResponse("Timer", "Time is:");
            Connection.Broadcast(data);
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            _clients.Add(connectionId, string.Empty);
            var chatData = new TeamChatResponse("Server", "A new user has joined the room.");
            return Connection.Broadcast(chatData);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            var chatData = JsonConvert.DeserializeObject<TeamChatPost>(data);
            _clients[connectionId] = chatData.Name;

            return Connection.Broadcast(TeamChatResponse.FromResponse(chatData));
        }

        protected override Task OnDisconnected(IRequest request, string connectionId)
        {
            var name = _clients[connectionId];
            var chatData = new TeamChatResponse("Server", string.Format("{0} has left the room.", name));
            
            _clients.Remove(connectionId);
            
            return Connection.Broadcast(chatData);
        }
    }
}