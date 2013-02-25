using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.CloseAsync().Wait();
        }

        [Test]
        public void CreateGameReturnsCreated()
        {
            var client = new HttpClient(_server);
            var request = CreateRequest
                ("api/Game", "application/json", HttpMethod.Post, new { name ="test", firstTo=3 }, new JsonMediaTypeFormatter());
         
            using (var response = client.SendAsync(request).Result)
            {
                Assert.IsNotNull(response);
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Assert.IsTrue(response.Headers.Location.OriginalString.StartsWith(_urlBase + "api/Game/"));
            }
        }
        
        private HttpRequestMessage CreateRequest(string url, string contentType, HttpMethod method)
        {
            var request = new HttpRequestMessage {RequestUri = new Uri(_urlBase + url)};
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            request.Method = method;

            return request;
        }

        private HttpRequestMessage CreateRequest<T>(string url, string contentType, HttpMethod method, T content, MediaTypeFormatter formatter) where T : class
        {
            //TODO contentType enum
            HttpRequestMessage request = CreateRequest(url, contentType, method);
            request.Content = new ObjectContent<T>(content, formatter);

            return request;
        }
        
    }
}
