using ECommerce.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories
{
    public interface IWriteRepository<T>:IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsycn(T entity);
        Task<bool> AddRangeAsycn(List<T> entities);
        bool Remove(T entity);
        Task<bool> RemoveAsycn(int id);
        bool RemoveRange(List<T> entities);
        bool Update(T entity);
        Task<int> SaveAsycn();
    }
}
