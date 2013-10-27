using System.Collections.Generic;
using System.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
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
        private readonly ISponsorMessageModelFactory _modelFactory;

        private readonly IHubConnectionContext _clientContext =
            GlobalHost.ConnectionManager.GetHubContext<TeamSauceHub>().Clients;

        public TeamSauceHub()
            : this(ServiceFactory.GetUserService(), new SponsorMessageService(), new SponsorMessageModelFactory())
        {
            
        }

        public TeamSauceHub(IUserService userService, ISponsorMessageService sponsorMessageService, 
            ISponsorMessageModelFactory modelFactory)
        {
            _userService = userService;
            _sponsorMessageService = sponsorMessageService;
            _modelFactory = modelFactory;
        }

        public void LogIn(string username, string password)
        {
            _userService.CreateUser(Context.ConnectionId, username, password);
        }

        public void MessageReceived(string message)
        {
            var sender = _userService.GetUser(Context.ConnectionId);

            var sponsorMessageModel = _modelFactory.Create(message, sender);

            _sponsorMessageService.PersistMessage(sponsorMessageModel);

            switch (sender.UserType)
            {
                case "User":
                    _clientContext.All.UserMessage(sponsorMessageModel);
                    break;
                case "Sponsor":
                    _clientContext.All.SponsorMessage(sponsorMessageModel);
                    break;
            }
        }

        public void GetMessages()
        {
            var messages = _sponsorMessageService.GetMessages();
            _clientContext.Client(Context.ConnectionId).MessagesLoaded(messages);
        }

        public void Complete(string questionnaireId, string data)
        {
            var questionnaireResponse = JsonConvert.DeserializeObject<QuestionnaireResponse>(data);

            var documentStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);
            var questionnaire = documentStore.FindQuestionnaire(questionnaireId);

            if (questionnaire.questionnaireresponses == null)
                questionnaire.questionnaireresponses = new List<QuestionnaireResponse> { questionnaireResponse };
            else
                questionnaire.questionnaireresponses.Add(questionnaireResponse);

            documentStore.UpsertQuestionnaire(questionnaire);
        }
    }
}