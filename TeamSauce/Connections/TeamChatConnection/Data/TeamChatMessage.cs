using System;

namespace TeamSauce.Connections.TeamChatConnection.Data
{
    public class TeamChatMessage
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public DateTime Time { get; set; }

        public TeamChatMessage()
        {
            Time = DateTime.UtcNow;
        }

        public TeamChatMessage(string name, string body)
        {
            Name = name;
            Body = body;
            Time = DateTime.UtcNow;
        }

        public static TeamChatMessage FromResponse(TeamChatPost model)
        {
            return new TeamChatMessage(model.Name, model.Body);
        }
    }
}