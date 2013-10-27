namespace TeamSauce.Models.Factories
{
    public interface ISponsorMessageModelFactory
    {
        SponsorMessageModel Create(string message, User user);
    }
}