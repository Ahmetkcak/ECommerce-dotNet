using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Common;
using ECommerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ECommerceDbContext _context; 
        public ReadRepository(ECommerceDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
            
        }
            
        public async Task<T> GetByIdAsycn(int id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if(!tracking) 
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(x => x.Id == id);
            
        }
           
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if(!tracking)
                query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(expression);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            var query = Table.Where(expression);
            if (!tracking)
                query = query.AsNoTracking();
            return query;
            
        }
    }
}
