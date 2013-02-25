namespace ES.Lab.Api.Infrastructure.Security
{
    public interface IRoleProvider
    {
        string[] GetRoles(string userName);
    }
}