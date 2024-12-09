namespace Patients.EventBus.Abstraction;

public interface IPOEventBus
{
    Task Publish<T>(T messageToPublish) where T : IntegrationMsg;
    Task Subscribe<T>(Action<T> callBack) where T : IntegrationMsg;

}
