using TeamSauce.Connections.TeamChatConnection.Data;
using TeamSauce.DataAccess.Model;

namespace TeamSauce.Services
{
    public class TeamChatModelTransformer
    {
        public TeamChatMessage FromDto(TeamMessageDto teamMessageDto)
        {
            return new TeamChatMessage(teamMessageDto.Sender, teamMessageDto.Message) { Time = teamMessageDto.Time };
        }

        public TeamMessageDto ToDto(TeamChatMessage message)
        {
            return new TeamMessageDto { Message = message.Body, Sender = message.Name, Time = message.Time };
        }
    }
}