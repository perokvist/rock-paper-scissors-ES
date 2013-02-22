using System.Web;
using ES.Lab.Api.Infrastructure;
using ES.Lab.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        public HttpResponseMessage CreateGame(JObject input)
        {
            //TODO async, remove xml support
            var gameId = Guid.NewGuid();
            var player = base.User.Identity.Name;
            ////TODO Validate command
            var command = new CreateGameCommand(gameId, player, input.Value<string>("name") ,input.Value<int>("firstTo"));
            _commandBus.Send(command);
            var response = Request.CreateResponse(HttpStatusCode.Created);
            var uri = Url.Link("DefaultApi", new { id = gameId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

    }
}
