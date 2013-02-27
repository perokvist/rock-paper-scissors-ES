using System.Threading.Tasks;
using Treefort;
using Treefort.Commanding;

namespace ES.Lab.Api.Infrastructure
{
    public class CommandBus : ICommandBus
    {
        private readonly IApplicationService _applicationService;

        public CommandBus(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public void Send(ICommand command)
        {
            _applicationService.Handle(command);
            //Task.Run(() => _applicationService.Handle(command));
        } 
    }
}