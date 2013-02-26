using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http.SelfHost;
using NUnit.Framework;

namespace ES.Lab.Api.IntegrationTests
{
    public abstract class WithSelfHost
    {
        private static HttpSelfHostServer _server;
        protected string UrlBase = "http://eslab.server/";

        
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


        protected void Send(HttpRequestMessage req, Action<HttpResponseMessage> assert)
        {
            var client = new HttpClient(_server);

            using (var response = client.SendAsync(req).Result)
            {
                assert(response);
            }
        }

        protected HttpRequestMessage CreateRequest(string url, string contentType, HttpMethod method, string user, string pass)
        {
            var request = new HttpRequestMessage { RequestUri = new Uri(UrlBase + url) };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            request.Method = method;
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
            Encoding.ASCII.GetBytes(string.Format("{0}:{1}", user, pass))));
            return request;
        }

        protected HttpRequestMessage CreateRequest<T>(string url, string contentType, HttpMethod method, T content, MediaTypeFormatter formatter,
            string user = null, string pass = null) where T : class
        {
            var request = CreateRequest(url, contentType, method, user, pass);
            request.Content = new ObjectContent<T>(content, formatter);

            return request;
        }

        protected static class MediaType
        {
            public const string Json = "application/json";
        }

    }
}