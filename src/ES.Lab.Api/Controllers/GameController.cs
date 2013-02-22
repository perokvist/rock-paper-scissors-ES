using System.Web;
using ES.Lab.Api.Infrastructure;
using ES.Lab.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ES.Lab.Api.Controllers
{
    public class GameController : ApiController
    {
        private readonly ICommandBus _commandBus;

        public GameController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }
        
        [HttpPost]
        public bool CreateGame()
        {
            var command = new CreateGameCommand(Guid.NewGuid(), "player", "newGame", 3);
            _commandBus.Send(command);
            return true;
        }

    }
}
