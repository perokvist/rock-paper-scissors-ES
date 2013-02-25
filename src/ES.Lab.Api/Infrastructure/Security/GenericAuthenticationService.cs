using System;

namespace ES.Lab.Api.Infrastructure.Security
{
    public class GenericAuthenticationService : IAuthenticationService
    {
        private readonly Func<string, string, bool> _provider;

        public GenericAuthenticationService(Func<string, string, bool> provider)
        {
            _provider = provider;
        }

        public bool Authenticate(string username, string password)
        {
            return _provider(username, password);
        }
    }
}