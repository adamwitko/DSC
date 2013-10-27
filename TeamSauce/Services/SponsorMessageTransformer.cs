using TeamSauce.DataAccess.Model;
using TeamSauce.Models;

namespace TeamSauce.Services
{
    public class SponsorMessageTransformer
    {
        public SponsorMessageDto ToDto(SponsorMessageModel model)
        {
            return new SponsorMessageDto
                {
                    Message = model.Message,
                    MessageType = model.MessageType,
                    Sender = model.Sender,
                    Time = model.Time
                };

        }

        public SponsorMessageModel ToModel(SponsorMessageDto dto)
        {
            return new SponsorMessageModel
                {
                    Message = dto.Message,
                    MessageType = dto.MessageType,
                    Sender = dto.Sender,
                    Time = dto.Time
                };
        }
    }
}