using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechsysLog.Core.Data;

namespace TechsysLog.Core.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IUnitOfWork UnitOfWork { get; }
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);
        void Add(T entity);
        Task Update(T entity);

        Task DeleteById(Guid id);
    }
}
