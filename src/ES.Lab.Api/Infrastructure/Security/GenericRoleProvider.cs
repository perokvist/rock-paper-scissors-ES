using System;

namespace ES.Lab.Api.Infrastructure.Security
{
    public class GenericRoleProvider : IRoleProvider
    {
        private readonly Func<string, string[]> _provider;

        public GenericRoleProvider(Func<string, string[]> provider)
        {
            _provider = provider;
        }

        public string[] GetRoles(string userName)
        {
            return _provider(userName);
        }
    }
}