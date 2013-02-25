using System.Threading.Tasks;
using System.Web;
using ES.Lab.Api.Infrastructure;
using ES.Lab.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ES.Lab.Infrastructure;
using Newtonsoft.Json.Linq;

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
        public async Task<HttpResponseMessage> CreateGame(JObject input)
        {
            //TODO remove xml support
            var gameId = Guid.NewGuid();
            ////TODO Validate command
            var command = new CreateGameCommand(gameId, base.User.Identity.Name, input.Value<string>("name") ,input.Value<int>("firstTo"));
            _commandBus.Send(command);
            return Request.CreateResponse(HttpStatusCode.Created)
                .Tap(r => r.Headers.Location = new Uri(Url.Link("DefaultApi", new {id = gameId})));
        }

    }
}
