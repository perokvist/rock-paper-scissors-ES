namespace ES.Lab.Read
{
    public interface IProjection
    {
        void When(IEvent @event);
    }
}