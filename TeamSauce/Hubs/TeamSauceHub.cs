using System.Collections.Generic;
using System.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using TeamSauce.Connections.TeamChatConnection.Data;
using TeamSauce.DataAccess;
using TeamSauce.DataAccess.Model;
using TeamSauce.Models.Factories;
using TeamSauce.Services;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Hubs
{
    [HubName("teamSauceHub")]
    public class TeamSauceHub : Hub
    {
        private readonly IUserService _userService;
        private readonly ISponsorMessageService _sponsorMessageService;
        private readonly ISponsorMessageModelFactory _sponsorMessageModelFactory;
        private readonly IQuestionnaireResultService _questionnaireResultService;
        private readonly ITeamMessageModelFactory _teamMessageModelFactory;
        private readonly ITeamChatMessageService _teamChatMessageService;

        private readonly IHubConnectionContext _teamSauceHubContext = GlobalHost.ConnectionManager.GetHubContext<TeamSauceHub>().Clients;


        public TeamSauceHub()
            : this(ServiceFactory.GetUserService(), new SponsorMessageService(), new SponsorMessageModelFactory(), 
            new QuestionnaireResultsService(), new TeamMessageModelFactory(), new TeamChatMessageService())
        {
        }

        public TeamSauceHub(IUserService userService, ISponsorMessageService sponsorMessageService, 
            ISponsorMessageModelFactory sponsorMessageModelFactory, IQuestionnaireResultService questionnaireResultService,
            ITeamMessageModelFactory teamMessageModelFactory, ITeamChatMessageService teamChatMessageService)
        {
            _userService = userService;
            _sponsorMessageService = sponsorMessageService;
            _sponsorMessageModelFactory = sponsorMessageModelFactory;
            _questionnaireResultService = questionnaireResultService;
            _teamMessageModelFactory = teamMessageModelFactory;
            _teamChatMessageService = teamChatMessageService;
        }

        public void LogIn(string username, string password)
        {
            _userService.CreateUser(Context.ConnectionId, username, password);
        }

        public void LogOut()
        {
        }

        public void MessageReceived(string message)
        {
            var sender = _userService.GetUser(Context.ConnectionId);

            var sponsorMessageModel = _sponsorMessageModelFactory.Create(message, sender);

            _sponsorMessageService.PersistMessage(sponsorMessageModel);

            switch (sender.UserType)
            {
                case "User":
                    _teamSauceHubContext.All.UserMessage(sponsorMessageModel);
                    break;
                case "Sponsor":
                    _teamSauceHubContext.All.SponsorMessage(sponsorMessageModel);
                    break;
            }
        }

        public void GetMessages()
        {
            var messages = _sponsorMessageService.GetMessages();
            _teamSauceHubContext.Client(Context.ConnectionId).MessagesLoaded(messages);
        }

        public void GetData()
        {
            var calculatedQuestionnaireResultAverages = _questionnaireResultService.GetData();
            Clients.Client(Context.ConnectionId).getData(calculatedQuestionnaireResultAverages);
        }

        public void Complete(string questionnaireId, string data)
        {
            if (questionnaireId == null)
                return;

            var questionnaireResponse = JsonConvert.DeserializeObject<QuestionnaireResponse>(data);

            var documentStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);
            var questionnaire = documentStore.FindQuestionnaire(questionnaireId);

            if (questionnaire.questionnaireresponses == null)
                questionnaire.questionnaireresponses = new List<QuestionnaireResponse> { questionnaireResponse };
            else
                questionnaire.questionnaireresponses.Add(questionnaireResponse);

            documentStore.UpsertQuestionnaire(questionnaire);

            var calculatedQuestionnaireResultAverages = _questionnaireResultService.GetData();

            _teamSauceHubContext.All.GetData(calculatedQuestionnaireResultAverages);
        }

        public void TeamMessageReceived(string message)
        {
            var sender = _userService.GetUser(Context.ConnectionId);
            var model = _teamMessageModelFactory.Create(message, sender.Name);

            var teamChatMessage = new TeamChatMessage(model.Sender, model.Message);
            _teamChatMessageService.AddMessage(teamChatMessage);

            _teamSauceHubContext.All.TeamMessages(new List<TeamChatMessage> { teamChatMessage });
        }

        public void GetTeamMessages()
        {
            var teamMessages = _teamChatMessageService.GetTeamMessages(Context.ConnectionId);
            _teamSauceHubContext.Client(Context.ConnectionId).TeamMessages(teamMessages);
        }
    }
}