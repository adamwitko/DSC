﻿using System.Collections.Generic;
using System.Configuration;
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

        private IHubConnectionContext _clientContext = GlobalHost.ConnectionManager.GetHubContext<TeamSauceHub>().Clients;

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
            var service = new UserDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            var foundUser = service.IsUserValid(username, password);
            
            if (foundUser == null)
            {
                _clientContext.Client(connectionId).LoginFailed(connectionId);
                return;
            }

            if (_currentUsers.All(user => user.ConnectionId != connectionId))
            {
                _currentUsers.Add(new User { ConnectionId = connectionId, Name = foundUser.Username, TeamId = foundUser.TeamId, UserType = foundUser.UserType});
            }

            _clientContext.Client(connectionId).Completed(connectionId);

        }
    }
}