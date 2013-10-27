using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TeamSauce.DataAccess;
using TeamSauce.DataAccess.Model;
using TeamSauce.Models;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Services
{
    public class SponsorMessageService : ISponsorMessageService
    {
        public void PersistMessage(SponsorMessageModel message)
        {
            using (var store = new SponsorMessageDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]))
            {
                store.PersistMessage(new SponsorMessageDto()
                    {
                        Message = message.Message,
                        MessageType = message.MessageType,
                        Sender = message.Sender,
                        Time = message.Time
                    });
            }
        }

        public IEnumerable<SponsorMessageModel> GetMessages()
        {
            using (var store = new SponsorMessageDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]))
            {
                var messages = store.GetMessages();

                return messages.Select(sponsorMessageDto => 
                    new SponsorMessageTransformer().ToModel(sponsorMessageDto))
                    .ToList();
            }
        }
    }
}