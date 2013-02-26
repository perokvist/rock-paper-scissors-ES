using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http.SelfHost;
using ES.Lab.Api.Infrastructure.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ES.Lab.Api.Infrastructure;
using System.Web.Http;

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
            var id = Guid.NewGuid();
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
