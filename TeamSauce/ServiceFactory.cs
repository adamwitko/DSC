using TeamSauce.Services;
using TeamSauce.Services.Interfaces;

namespace TeamSauce
{
    public static class ServiceFactory
    {
        private static ITeamChatMessageService _mockTeamChatMessageService;

        private static IUserService _userService;

        public static ITeamChatMessageService GetTeamChatMessageService()
        {
            return _mockTeamChatMessageService ?? (_mockTeamChatMessageService = new TeamChatMessageService());
        }

        public static IUserService GetUserService()
        {
            return _userService ?? (_userService = new UserService());
        }
    }
}