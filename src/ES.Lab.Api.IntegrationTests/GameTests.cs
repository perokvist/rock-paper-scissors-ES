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
    public class GameTests
    {
        private static HttpSelfHostServer _server;
        private const string UrlBase = "http://eslab.server/";

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            var config = new HttpSelfHostConfiguration(UrlBase);
            WebApiApplication.Start(config);
            _server = new HttpSelfHostServer(config);
            _server.OpenAsync().Wait();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.CloseAsync().Wait();
        }

        [Test]
        public void CreateGameRequiresAuth()
        {
            //TODO all routes
            var client = new HttpClient(_server);
            var request = CreateRequest
                ("api/Game", "application/json", HttpMethod.Post, new { name = "test", firstTo = 3 },
                new JsonMediaTypeFormatter());

            using (var response = client.SendAsync(request).Result)
            {
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Test]
        public void CreateGameReturnsCreated()
        {
            var client = new HttpClient(_server);
            var request = CreateRequest
                ("api/Game", "application/json", HttpMethod.Post, new { name = "test", firstTo = 3 }, 
                new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");

            using (var response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Assert.IsTrue(response.Headers.Location.OriginalString.StartsWith(UrlBase + "api/Game/"));
            }
        }


        [Test]
        public void JoinGameReturnsOk()
        {
            var client = new HttpClient(_server);
            var request = CreateRequest
                (string.Format("api/Game/{0}/opponent", Guid.NewGuid()), "application/json", HttpMethod.Post, new {},
                new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");

            using (var response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsTrue(response.Headers.Location.OriginalString.StartsWith(UrlBase + "api/Game/"));
            }
        }

        [Test]
        public void ChoiceReturnsOk()
        {
            var client = new HttpClient(_server);
            var request = CreateRequest
                (string.Format("api/Game/{0}/choice", Guid.NewGuid()), "application/json", 
                HttpMethod.Post, new { choice = "2"},
                new JsonMediaTypeFormatter(), "test@jayway.com", "eslab");

            using (var response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsTrue(response.Headers.Location.OriginalString.StartsWith(UrlBase + "api/Game/"));
            }
        }


        private HttpRequestMessage CreateRequest(string url, string contentType, HttpMethod method, string user, string pass)
        {
            var request = new HttpRequestMessage { RequestUri = new Uri(UrlBase + url) };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            request.Method = method;
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
            Encoding.ASCII.GetBytes(string.Format("{0}:{1}", user, pass))));
            return request;
        }

        private HttpRequestMessage CreateRequest<T>(string url, string contentType, HttpMethod method, T content, MediaTypeFormatter formatter,
            string user = null, string pass = null) where T : class
        {
            var request = CreateRequest(url, contentType, method, user, pass);
            request.Content = new ObjectContent<T>(content, formatter);

            return request;
        }

    }
}
