namespace EventsSubscriber.Interfaces
{
    public interface IRabbitConfigurationProvider
    {
        string GetHostName();
        string GetUserName();
        string GetPassword();
    }
}