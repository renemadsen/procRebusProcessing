using System.Threading.Tasks;

namespace Microting.RebusPOC.Service.Managers
{
    public interface IMasterManager
    {
        Task Trigger(long id);
    }
}