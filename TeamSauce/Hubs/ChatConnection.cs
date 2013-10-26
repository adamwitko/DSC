using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using TeamSauce.Models;

namespace TeamSauce.Hubs
{
    
    public class ChatConnection : PersistentConnection
    {
        private Dictionary<string, string> _clients = new Dictionary<string, string>();


        protected override Task OnConnected(IRequest request, string connectionId)
        {
            _clients.Add(connectionId, string.Empty);
            var chatData = new ChatData("Server", "A new user has joined the room.");
            return Connection.Broadcast(chatData);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            var chatData = JsonConvert.DeserializeObject<ChatData>(data);
            _clients[connectionId] = chatData.Name;
            return Connection.Broadcast(chatData);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId)
        {
            var name = _clients[connectionId];
            var chatData = new ChatData("Server", string.Format("{0} has left the room.", name));
            _clients.Remove(connectionId);
            return Connection.Broadcast(chatData);
        }
    }
}