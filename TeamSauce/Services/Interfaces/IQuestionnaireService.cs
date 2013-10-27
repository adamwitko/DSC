using TeamSauce.DataAccess.Model;

namespace TeamSauce.Services.Interfaces
{
    public interface IQuestionnaireService
    {
        void Upsert(string id, QuestionnaireResponse response);
    }
}