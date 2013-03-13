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
using ES.Lab.Read;
using Newtonsoft.Json.Linq;
using Treefort.Infrastructure;
using Treefort.EntityFramework.Eventing;

namespace ES.Lab.Api.Controllers
{
  
    [Authorize(Roles = "Player")]
    public class GameController : ApiController
    {
        private readonly ICommandBus _commandBus;
        private readonly IGameViews _gameViews;
        private readonly IEventContext _context;

        public GameController(ICommandBus commandBus, IGameViews gameViews, IEventContext context) 
        {
            _commandBus = commandBus;
            _gameViews = gameViews;
            _context = context;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateGame(JObject input)
        {
            //TODO remove xml support
            var gameId = Guid.NewGuid();
            var command = new CreateGameCommand(gameId, User.Identity.Name, input.Value<string>("name"), input.Value<int>("firstTo"));
            await _commandBus.SendAsync(command);
            return Request.CreateResponse(HttpStatusCode.Created)
                .Tap(r => r.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = gameId })));
        }

        [HttpPost, ActionName("opponent")]
        public async Task<HttpResponseMessage> JoinGame([FromUri]Guid id)
        {
            await _commandBus.SendAsync(new JoinGameCommand(id, User.Identity.Name));
            return Request.CreateResponse(HttpStatusCode.OK).Tap(
                    r => r.Headers.Location = new Uri(Url.Link("DefaultApi", new {id = id})));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Choice([FromUri]Guid id, JObject input)
        {
            Choice choice; //TODO remove domain enum here and in command...
            if(!Enum.TryParse(input.Value<string>("choice"), out choice))
                throw new ArgumentException();

            await _commandBus.SendAsync(new MakeChoiceCommand(id, User.Identity.Name, choice));
            return Request.CreateResponse(HttpStatusCode.OK).Tap(
                    r => r.Headers.Location = new Uri(Url.Link("DefaultApi", new {id})));
        }

        public async Task<GameDetails> GetGame([FromUri]Guid id)
        {
            return _gameViews.GetGameDetails(id);
        }   

        [Queryable]
        public async Task<IQueryable<OpenGame>> GetOpenGames()
        {
            return _gameViews.GetOpenGames().AsQueryable();
        }  
    }
}
