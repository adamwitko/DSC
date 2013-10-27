using System.Collections.Generic;

namespace TeamSauce.Models
{
    public class UserQuestionnaire
    {
        public string id { get; set; }
        public IList<CategoryQuestion> categoryQuestions { get; set; }
    }
}