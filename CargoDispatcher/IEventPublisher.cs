namespace CargoDispatcher
{
    public interface IEventPublisher
    {
        void Publish(params Event[] events);
    }
}