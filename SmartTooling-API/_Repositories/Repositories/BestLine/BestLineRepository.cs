using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API.Data;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.BestLine;

namespace SmartTooling_API._Repositories.Repositories.BestLine
{
    public class BestLineRepository<T> : IBestLineRepository<T> where T : class
    {
        public string DataSeach;
        private readonly DataContext _context;
        private readonly CBDataContext _CBcontext;
        private readonly SHCDataContext _SHCcontext;
        private readonly SPCDataContext _SPCcontext;
        private readonly TSHDataContext _TSHcontext;

        private IConfiguration _configuration;
        public BestLineRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext,
                            SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration)
        {
            _context = context;
            _CBcontext = CBcontext;
            _SHCcontext = SHCcontext;
            _SPCcontext = SPCcontext;
            _TSHcontext = TSHcontext;
            _configuration = configuration;
        }
        public void Add(T entity)
        {
            DataSeach = _configuration.GetSection("AppSettings:DataSeach").Value;
            if (DataSeach.Trim() == "CB")
            {
                _CBcontext.Add(entity);
            }
            if (DataSeach.Trim() == "SHC")
            {
                _SHCcontext.Add(entity);
            }
            if (DataSeach.Trim() == "SPC")
            {
                _SPCcontext.Add(entity);
            }
            if (DataSeach.Trim() == "TSH")
            {
                _TSHcontext.Add(entity);
            }
            else
            {
                _context.Add(entity);
            }
        }

        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            DataSeach = _configuration.GetSection("AppSettings:DataSeach").Value;
            IQueryable<T> items = _context.Set<T>();
            if (DataSeach.Trim() == "CB")
            {
                items = _CBcontext.Set<T>();
            }
            if (DataSeach.Trim() == "SHC")
            {
                items = _SHCcontext.Set<T>();
            }
            if (DataSeach.Trim() == "SPC")
            {
                items = _SPCcontext.Set<T>();
            }
            if (DataSeach.Trim() == "TSH")
            {
                items = _TSHcontext.Set<T>();
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.AsQueryable();
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            DataSeach = _configuration.GetSection("AppSettings:DataSeach").Value;
            IQueryable<T> items = _context.Set<T>();
            if (DataSeach.Trim() == "CB")
            {
                items = _CBcontext.Set<T>();
            }
            if (DataSeach.Trim() == "SHC")
            {
                items = _SHCcontext.Set<T>();
            }
            if (DataSeach.Trim() == "SPC")
            {
                items = _SPCcontext.Set<T>();
            }
            if (DataSeach.Trim() == "TSH")
            {
                items = _TSHcontext.Set<T>();
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate).AsQueryable();
        }

        public T FindById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Remove(object id)
        {
            Remove(FindById(id));
        }

        public void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task<bool> SaveAll()
        {
            DataSeach = _configuration.GetSection("AppSettings:DataSeach").Value;
            if (DataSeach.Trim() == "CB")
            {
                return await _CBcontext.SaveChangesAsync() > 0;
            }
            if (DataSeach.Trim() == "SHC")
            {
                return await _SHCcontext.SaveChangesAsync() > 0;
            }
            if (DataSeach.Trim() == "SPC")
            {
                return await _SPCcontext.SaveChangesAsync() > 0;
            }
            if (DataSeach.Trim() == "TSH")
            {
                return await _TSHcontext.SaveChangesAsync() > 0;
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void AddMultiple(List<T> entities)
        {
            _context.AddRange(entities);
        }

        public void UpdateMultiple(List<T> entities)
        {
            _context.UpdateRange(entities);
        }
    }
}