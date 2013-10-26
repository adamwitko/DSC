using System;

namespace TeamSauce.Connections.TeamChatConnection.Data
{
    public class TeamChatResponse
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public DateTime Time { get; set; }

        public TeamChatResponse()
        {
            Time = DateTime.UtcNow;
        }

        public TeamChatResponse(string name, string body)
        {
            Name = name;
            Body = body;
            Time = DateTime.UtcNow;
        }

        public static TeamChatResponse FromResponse(TeamChatPost model)
        {
            return new TeamChatResponse(model.Name, model.Body);
        }
    }
}