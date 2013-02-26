using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ES.Lab.Api.Infrastructure.Security
{
    public class BasicAuthenticationMessageHandler : DelegatingHandler
    {
        private readonly IPrincipalProvider _principalProvider;
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";

        public BasicAuthenticationMessageHandler(IPrincipalProvider principalProvider)
        {
            _principalProvider = principalProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var authValue = request.Headers.Authorization;
            if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter))
            {
                var parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);
                if (parsedCredentials != null)
                {
                    Thread.CurrentPrincipal = _principalProvider
                        .CreatePrincipal(parsedCredentials.Username, parsedCredentials.Password);
                }
            }

            return base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;
                    if (response.StatusCode == HttpStatusCode.Unauthorized
                        && !response.Headers.Contains(BasicAuthResponseHeader))
                    {
                        response.Headers.Add(BasicAuthResponseHeader
                                             , BasicAuthResponseHeaderValue);
                    }
                    return response;
                });
        }

        private static Credentials ParseAuthorizationHeader(string authHeader)
        {
            var credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader))
                .Split(
                    new[] { ':' });
            
            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0])
                || string.IsNullOrEmpty(credentials[1])) return null;

            return new Credentials()
            {
                Username = credentials[0],
                Password = credentials[1],
            };
        }
    }
}