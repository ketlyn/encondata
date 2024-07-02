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
    public interface IOrderRepository : IRepository<Order>
    {
        Task<PagedResult<Order>> GetAllOrdersPaged(int pageSize, int pageIndex, string query = null);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly TechsysLogDBContext _context;
        public OrderRepository(TechsysLogDBContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;
        public async Task<PagedResult<Order>> GetAllOrdersPaged(int pageSize, int pageIndex, string query = null)
        {
            var sql = @$"SELECT * FROM Order 
                          WHERE UpdateDate >= @CurrentDate 
                          ORDER BY [Number] 
                          OFFSET {pageSize * (pageIndex - 1)} ROWS 
                          FETCH NEXT {pageSize} ROWS ONLY; 
              
                          SELECT COUNT(Id) FROM Order 
                          WHERE UpdateDate >= @CurrentDate";

            var multi = await _context.Database.GetDbConnection()
                .QueryMultipleAsync(sql, new { Name = query });

            var orders = multi.Read<Order>();
            var total = multi.Read<int>().FirstOrDefault();

            return new PagedResult<Order>()
            {
                List = orders,
                TotalResults = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };
        }
        public void Add(Order entity)
        {
            _context.Orders.Add(entity);
        }
        public async Task Update(Order entity)
        {
            _context.Orders.Update(entity);
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
        public async Task<Order> GetById(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }
        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.AsNoTracking().ToListAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Remove(order);
        }
    }
}

