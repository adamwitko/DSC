namespace TeamSauce.Connections.TeamChatConnection.Data
{
    public class TeamChatPost
    {
        public string Name { get; set; }
        public string Body { get; set; }

        public TeamChatPost(string name, string body)
        {
            Name = name;
            Body = body;
        }
    }
}