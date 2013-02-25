using System.Security.Principal;

namespace ES.Lab.Api.Infrastructure.Security
{
    public interface IPrincipalProvider
    {
        IPrincipal CreatePrincipal(string username, string password);
    }
}