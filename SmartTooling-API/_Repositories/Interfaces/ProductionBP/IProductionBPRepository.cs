using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartTooling_API._Repositories.Interfaces.ProductionBP
{
    public interface IProductionBPRepository<T> where T : class
    {
        T FindById(object id);

        T FindSingle(Expression<Func<T, bool>> predicate);

        IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        void Remove(object id);

        void RemoveMultiple(List<T> entities);

        IQueryable<T> GetAll();

        Task<bool> SaveAll();

        void UpdateMultiple(List<T> entities);
        void AddMultiple(List<T> entities);
    }
}