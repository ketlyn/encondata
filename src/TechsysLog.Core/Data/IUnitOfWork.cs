using System.Threading.Tasks;

namespace TechsysLog.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
