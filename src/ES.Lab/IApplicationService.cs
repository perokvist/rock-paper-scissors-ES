namespace ES.Lab
{
    public interface IApplicationService
    {
        void Handle(ICommand command);
    }
}