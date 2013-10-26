using TeamSauce.Services;
using TeamSauce.Services.Interfaces;

namespace TeamSauce
{
    public static class ServiceFactory
    {
        public static ITeamChatMessageService GetTeamChatMessageService()
        {
            return new MockTeamChatMessageService();
        }
    }
}