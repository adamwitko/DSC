using TeamSauce.Services;
using TeamSauce.Services.Interfaces;

namespace TeamSauce
{
    public static class ServiceFactory
    {
        private static MockTeamChatMessageService _mockTeamChatMessageService;

        private static IUserService _userService;

        public static ITeamChatMessageService GetTeamChatMessageService()
        {
            return _mockTeamChatMessageService ?? (_mockTeamChatMessageService = new MockTeamChatMessageService());
        }

        public static IUserService GetUserService()
        {
            return _userService ?? (_userService = new UserService());
        }
    }
}