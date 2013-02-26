using System.Diagnostics;
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
using ES.Lab.Domain;
using ES.Lab.Infrastructure;
using ES.Lab.Read;
using Newtonsoft.Json.Linq;

namespace ES.Lab.Api.Controllers
{
  
    [Authorize(Roles = "Player")]
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
            var command = new CreateGameCommand(gameId, User.Identity.Name, input.Value<string>("name"), input.Value<int>("firstTo"));
            _commandBus.Send(command);
            return Request.CreateResponse(HttpStatusCode.Created)
                .Tap(r => r.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = gameId })));
        }

        [HttpPost, ActionName("opponent")]
        public async Task<HttpResponseMessage> JoinGame([FromUri]Guid id)
        {
            _commandBus.Send(new JoinGameCommand(id, User.Identity.Name));
            return
                Request.CreateResponse(HttpStatusCode.OK).Tap(
                    r => r.Headers.Location = new Uri(Url.Link("DefaultApi", new {id = id})));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Choice([FromUri]Guid id, JObject input)
        {
            var choice = input.Value<int>("choice");
            if(choice<= 0)
                throw new ArgumentException();
            _commandBus.Send(new MakeChoiceCommand(id, User.Identity.Name,(Choice) choice));
            return
                Request.CreateResponse(HttpStatusCode.OK).Tap(
                    r => r.Headers.Location = new Uri(Url.Link("DefaultApi", new {id})));
        }

        public async Task<GameDetails> GetGame([FromUri]Guid id)
        {
            //TODO impl
            return new GameDetails(id, "some player");
        }   
    }
}
