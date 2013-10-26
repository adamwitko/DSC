using TeamSauce.Services;
using TeamSauce.Services.Interfaces;

namespace TeamSauce
{
    public static class ServiceFactory
    {
        private static MockTeamChatMessageService _mockTeamChatMessageService;

        public static ITeamChatMessageService GetTeamChatMessageService()
        {
            if (_mockTeamChatMessageService == null)
            {
                _mockTeamChatMessageService = new MockTeamChatMessageService();
            }

            return _mockTeamChatMessageService;
        }
    }
}