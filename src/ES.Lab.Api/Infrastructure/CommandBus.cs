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

        public async Task SendAsync(ICommand command)
        {
            //TODO fire and forget
            await _applicationService.HandleAsync(command);
        }
    }
}