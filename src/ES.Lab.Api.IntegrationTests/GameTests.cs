using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.SelfHost;
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
        private const string _urlBase = "http://eslab.server/";

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            var config = new HttpSelfHostConfiguration(_urlBase) {DependencyResolver = Bootstrapper.Start()};
            WebApiConfig.Register(config); 
            _server = new HttpSelfHostServer(config);
            _server.OpenAsync().Wait();
        }

        [Test]
        public void CreateGameReturns200()
        {
            var client = new HttpClient(_server);
            var request = CreateRequest("api/Game", "application/json", HttpMethod.Post);

            using (var response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(response.Content);
            }
        }

        private HttpRequestMessage CreateRequest(string url, string contentType, HttpMethod method)
        {
            var request = new HttpRequestMessage {RequestUri = new Uri(_urlBase + url)};
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            request.Method = method;

            return request;
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.CloseAsync().Wait();
        }

    }
}
