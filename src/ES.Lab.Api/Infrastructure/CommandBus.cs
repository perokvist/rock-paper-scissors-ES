using System.Threading.Tasks;
using ES.Lab.Commands;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ES.Lab.Infrastructure;
using Autofac;

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
            Task.Run(() => _applicationService.Handle(command));
        } 
    }
}