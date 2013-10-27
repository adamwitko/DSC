using System.Collections.Generic;

namespace TeamSauce.Services.Interfaces
{
    public interface IQuestionnaireResultService
    {
        IEnumerable<IDictionary<string, object>> GetData();
    }
}