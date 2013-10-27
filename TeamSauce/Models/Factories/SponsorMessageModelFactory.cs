using System;

namespace TeamSauce.Models.Factories
{
    public class SponsorMessageModelFactory : ISponsorMessageModelFactory
    {
        public SponsorMessageModel Create(string message, User user)
        {
            return new SponsorMessageModel
                {
                    Message = message,
                    MessageType = user.UserType,
                    Sender = user.Name,
                    Time = DateTime.UtcNow,
                    TeamId = user.TeamId
                };

        }
    }
}