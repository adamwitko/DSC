using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TeamSauce.DataAccess;
using TeamSauce.Exceptions;
using TeamSauce.Hubs;
using TeamSauce.Models;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Services
{
    public class UserService : IUserService
    {
        private List<User> _currentUsers = new List<User>();

        private IHubConnectionContext _clientContext = GlobalHost.ConnectionManager.GetHubContext<UsersHub>().Clients;

        public User GetUser(string connectionId)
        {
            if (_currentUsers.Any(user => user.ConnectionId == connectionId))
            {
                return _currentUsers.First(user => user.ConnectionId == connectionId);
            }

            throw new UserNotFoundException();
        }

        public void CreateUser(string connectionId, string username, string password)
        {
            var service = new UserDocumentStore();

            if(!service.IsUserValid(username, password))
                _clientContext.Client(connectionId).LoginFailed(connectionId);

            if (_currentUsers.All(user => user.ConnectionId != connectionId))
            {
                _currentUsers.Add(new User { ConnectionId = connectionId, Name = username });
            }

            _clientContext.Client(connectionId).Completed(connectionId);

        }
    }
}