namespace ES.Lab.Api.Infrastructure
{
    public interface ICommandBus
    {
        void Send(ICommand command);
    }
}