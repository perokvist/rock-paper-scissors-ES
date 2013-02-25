using System.Security.Principal;

namespace ES.Lab.Api.Infrastructure.Security
{
    public class PrincipalProvider : IPrincipalProvider
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRoleProvider _roleProvider;

        public PrincipalProvider(IAuthenticationService authenticationService, IRoleProvider roleProvider)
        {
            _authenticationService = authenticationService;
            _roleProvider = roleProvider;
        }

        public IPrincipal CreatePrincipal(string userName, string password)
        {
            if(_authenticationService.Authenticate(userName, password))
            {
                var identity = new GenericIdentity(userName);
                IPrincipal principal = new GenericPrincipal(identity, _roleProvider.GetRoles(userName));
                return principal;    
            }
            return null;
        }
    }
}