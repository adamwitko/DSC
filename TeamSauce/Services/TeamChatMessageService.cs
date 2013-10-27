using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TeamSauce.Connections.TeamChatConnection.Data;
using TeamSauce.DataAccess;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Services
{
    public class TeamChatMessageService : ITeamChatMessageService 
    {
        public IEnumerable<TeamChatMessage> GetTeamMessages(string connectionId)
        {
            var modelTransformer = new TeamChatModelTransformer();

            var store = new TeamMessageDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            var messageDtos = store.GetMessages();
            return messageDtos.Select(modelTransformer.FromDto).ToList();
        }

        public void AddMessage(TeamChatMessage message)
        {
            var modelTransformer = new TeamChatModelTransformer();

            var store = new TeamMessageDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);
            store.PersistMessage(modelTransformer.ToDto(message));
        }
    }
}