using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using ES.Lab.Read;
using NUnit.Framework;
using System;
using System.Linq;

namespace ES.Lab.Api.IntegrationTests
{
    /// <remarks>Needs admin rights</remarks>
    /// <remarks>http://stackoverflow.com/questions/2583347/c-sharp-httplistener-without-using-netsh-to-register-a-uri</remarks>
    public class GameTests : WithSelfHost
    {
        [Test]
        public void CreateGameRequiresAuth()
        {
            //TODO all routes
            var request = CreateRequest
                ("api/Game", MediaType.Json, HttpMethod.Post, new { name = "test", firstTo = 3 },
                new JsonMediaTypeFormatter());

            Send(request, res => Assert.AreEqual(HttpStatusCode.Unauthorized, res.StatusCode));
        }

        [Test]
        public void CreateGameReturnsCreated()
        {
            var request = CreateRequest
                ("api/Game", MediaType.Json, HttpMethod.Post, new { name = "test", firstTo = 3 },
                new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");

            Send(request, res => res.Tap(Assert.IsNotNull)
                .Tap(r => Assert.AreEqual(HttpStatusCode.Created, r.StatusCode))
                .Tap(r => Assert.IsTrue(r.Headers.Location.OriginalString.StartsWith(UrlBase + "api/Game/"))));
        }

        [Test]
        public void JoinGameReturnsOk()
        {
            var id = Guid.Parse("d12830d4-b498-4ac4-810e-fb47ac8b2f4b");
            var request = CreateRequest
                (string.Format("api/Game/{0}/opponent", id), MediaType.Json, HttpMethod.Post, new { },
                new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");


            Send(request, res => res.Tap(Assert.IsNotNull)
                .Tap(r => Assert.AreEqual(HttpStatusCode.OK, r.StatusCode))
                .Tap(r => Assert.IsTrue(r.Headers.Location.OriginalString.StartsWith(UrlBase + "api/Game/" + id))));
        }

        [Test]
        public void ChoiceReturnsOk()
        {
            var id = Guid.NewGuid();
            var request = CreateRequest
                (string.Format("api/Game/{0}/choice", id), MediaType.Json,
                HttpMethod.Post, new { choice = "2" },
                new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");

            Send(request, res => res.Tap(Assert.IsNotNull)
                .Tap(r => Assert.AreEqual(HttpStatusCode.OK, r.StatusCode))
                .Tap(r => Assert.IsTrue(r.Headers.Location.OriginalString.StartsWith(UrlBase + "api/Game/" + id))));

        }

        [Test]
        public void EndToEnd()
        {
            var createGamerequest = CreateRequest
                ("api/Game", MediaType.Json, HttpMethod.Post, new { name = "GameAsyncEndToEnd", firstTo = 1 },
                new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");

            var id = string.Empty;
            Send(createGamerequest, r => id = r.Headers.Location.OriginalString.Split('/').Last());
            id = Guid.Parse(id).ToString();

            var joinGamerequest = CreateRequest
                (string.Format("api/Game/{0}/opponent", id), MediaType.Json, HttpMethod.Post, new { },
                new JsonMediaTypeFormatter(), "test2@jayway.com", "eslab");
            var choicePlayer1Request = CreateRequest(string.Format("api/Game/{0}/choice", id), MediaType.Json,
                HttpMethod.Post, new { choice = "2" }, new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");
            var choicePlayer2Request = CreateRequest(string.Format("api/Game/{0}/choice", id), MediaType.Json,
                HttpMethod.Post, new { choice = "1" }, new JsonMediaTypeFormatter(), "test2@jayway.com", "eslab");

            var gameRequest = CreateRequest
                (string.Format("api/Game/{0}", id), MediaType.Json, HttpMethod.Get, new { },
                new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");
        
            Send(joinGamerequest, g => { });
            Send(choicePlayer1Request, g => { });
            Send(choicePlayer2Request, g => { });
            Send(gameRequest, r =>
                                  {
                                      var gd = r.Content.ReadAsAsync<GameDetails>().Result;
                                      Assert.AreEqual(1, gd.Rounds.Count());
                                      Assert.AreEqual("GameAsyncEndToEnd", gd.Title);
                                      Assert.AreEqual("test2@jayway.com", gd.WinnerId);
                                      Assert.AreEqual(HttpStatusCode.OK, r.StatusCode);
                                  }
            );
        }

        [Test]
        public void GetGameReturnsOk()
        {
            var request = CreateRequest
                (string.Format("api/Game/{0}", Guid.NewGuid()), MediaType.Json, HttpMethod.Get, new { },
                new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");

            Send(request, res => res.Tap(Assert.IsNotNull)
                                     .Tap(r => Assert.AreEqual(HttpStatusCode.OK, r.StatusCode)));
        }


        [Test]
        public void GetOpenGamesReturnsOk()
        {
            var request = CreateRequest
                ("api/Game/", MediaType.Json, HttpMethod.Get, new { },
                new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");

            Send(request, res => res.Tap(Assert.IsNotNull)
                                     .Tap(r => Assert.AreEqual(HttpStatusCode.OK, r.StatusCode)));
        }

    }
}
