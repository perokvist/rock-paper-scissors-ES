using System.Threading.Tasks;
using Treefort.Commanding;
namespace ES.Lab.Api.Infrastructure
{
    public interface ICommandBus
    {
        Task SendAsync(ICommand command);
    }
}