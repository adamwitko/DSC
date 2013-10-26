using System.Collections.Generic;
using System.Linq;
using TeamSauce.Exceptions;
using TeamSauce.Models;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Services
{
    public class UserService : IUserService
    {
        private List<User> _currentUsers = new List<User>();
        
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
            _currentUsers.Add(new User { ConnectionId = connectionId, Name = username});
        }
    }
}