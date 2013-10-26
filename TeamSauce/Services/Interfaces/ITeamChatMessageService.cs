using System.Collections.Generic;
using TeamSauce.Connections.TeamChatConnection.Data;

namespace TeamSauce.Services.Interfaces
{
    public interface ITeamChatMessageService
    {
        IEnumerable<TeamChatMessage> GetTeamMessages(string connectionId);
        void AddMessage(TeamChatMessage message);
    }
}