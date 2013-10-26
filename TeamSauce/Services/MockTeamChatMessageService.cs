using System;
using System.Collections.Generic;
using TeamSauce.Connections.TeamChatConnection.Data;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Services
{
    public class MockTeamChatMessageService : ITeamChatMessageService
    {
        private readonly List<TeamChatMessage> _messages = new List<TeamChatMessage>();

        public MockTeamChatMessageService()
        {
            _messages.Add(new TeamChatMessage("Sample1", "Message1") { Time = DateTime.UtcNow.AddHours(-1)});
            _messages.Add(new TeamChatMessage("Sample2", "Message2") { Time = DateTime.UtcNow.AddMinutes(-30)});
            _messages.Add(new TeamChatMessage("Sample3", "Message3") { Time = DateTime.UtcNow.AddMinutes(-15)});
            _messages.Add(new TeamChatMessage("Sample4", "Message4") { Time = DateTime.UtcNow.AddSeconds(-5)});
        }

        public IEnumerable<TeamChatMessage> GetTeamMessages(string connectionId)
        {
            return _messages;
        }

        public void AddMessage(TeamChatMessage message)
        {
            _messages.Add(message);
        }
    }
}