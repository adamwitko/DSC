namespace TeamSauce.Models.Factories
{
    public interface ITeamMessageModelFactory
    {
        TeamMessageModel Create(string message, string username);
    }
}