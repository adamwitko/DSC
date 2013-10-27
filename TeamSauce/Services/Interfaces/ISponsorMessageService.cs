using System.Collections.Generic;
using TeamSauce.Models;

namespace TeamSauce.Services.Interfaces
{
    public interface ISponsorMessageService
    {
        void PersistMessage(SponsorMessageModel message);
        IEnumerable<SponsorMessageModel> GetMessages();
    }
}