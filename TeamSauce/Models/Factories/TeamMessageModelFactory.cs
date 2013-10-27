using System;

namespace TeamSauce.Models.Factories
{
    public class TeamMessageModelFactory : ITeamMessageModelFactory
    {
        public TeamMessageModel Create(string message, string username)
        {
            return new TeamMessageModel { Message = message, Sender = username, Time = DateTime.UtcNow};
        }
    }
}