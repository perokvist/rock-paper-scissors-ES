namespace ES.Lab.Api.Infrastructure.Security
{
    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);
    }
}