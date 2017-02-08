namespace Common.Interfaces
{
    public interface IEventPublisher
    {
        void Publish(IEvent applicationEvent);
    }
}