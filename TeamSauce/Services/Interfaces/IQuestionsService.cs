using System.Collections.Generic;

namespace TeamSauce.Services.Interfaces
{
    public class IQuestionsService
    {
        public IEnumerable<Question> GetRandomQuestions()
        {
            return new List<Question>();
        }
    }

    public class Question
    {
    }
}