using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechsysLog.Api.Data;
using TechsysLog.Api.Entities;
using TechsysLog.Core.Data;
using TechsysLog.Core.Extensions;
using TechsysLog.Core.Interfaces;

namespace TechsysLog.Api.Repositories
{
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        Task<PagedResult<Delivery>> GetAllDeliveriesPaged(int pageSize, int pageIndex, string query = null);
    }
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly TechsysLogDBContext _context;
        public DeliveryRepository(TechsysLogDBContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;
        public async Task<PagedResult<Delivery>> GetAllDeliveriesPaged(int pageSize, int pageIndex, string query = null)
        {
            var sql = @$"SELECT * FROM Delivery 
                      WHERE (@DeliveryDate IS NOT NULL) 
                      ORDER BY [Number] 
                      OFFSET {pageSize * (pageIndex - 1)} ROWS 
                      FETCH NEXT {pageSize} ROWS ONLY 
                      SELECT COUNT(Id) FROM Delivery 
                      WHERE (@DeliveryDate IS NOT NULL)";

            var multi = await _context.Database.GetDbConnection()
                .QueryMultipleAsync(sql, new { Name = query });

            var deliverys = multi.Read<Delivery>();
            var total = multi.Read<int>().FirstOrDefault();

            return new PagedResult<Delivery>()
            {
                List = deliverys,
                TotalResults = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };
        }
        public void Add(Delivery entity)
        {
            _context.Deliveries.Add(entity);
        }
        public async Task Update(Delivery entity)
        {
            _context.Deliveries.Update(entity);
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
        public async Task<Delivery> GetById(Guid id)
        {
            return await _context.Deliveries.FindAsync(id);
        }
        public async Task<IEnumerable<Delivery>> GetAll()
        {
            return await _context.Deliveries.AsNoTracking().ToListAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            _context.Remove(delivery);
        }
    }
}

