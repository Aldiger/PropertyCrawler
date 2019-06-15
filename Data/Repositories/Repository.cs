using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PropertyCrawler.Data
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppContext Context;
        protected const int _pageSize = 8;

        public Repository(AppContext context)
        {
            Context = context;
        }

        public void Add(TEntity entity)
        {
            if (entity.GetType().GetProperty("DateModified") != null)
            {
                entity.GetType().GetProperty("DateModified").SetValue(entity, DateTime.UtcNow);
            }
            if(entity.GetType().GetProperty("Active") != null)
            {
                entity.GetType().GetProperty("Active").SetValue(entity, true);
            }
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.GetType().GetProperty("DateModified") != null)
                {
                    entity.GetType().GetProperty("DateModified").SetValue(entity, DateTime.UtcNow);
                }
                if (entity.GetType().GetProperty("Active") != null)
                {
                    entity.GetType().GetProperty("Active").SetValue(entity, true);
                }
            }
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Update(TEntity entity,int key)
        {
            if (entity == null)
                return;

            var existing = Context.Set<TEntity>().Find(key);

            if (existing != null)
            {
                if (entity.GetType().GetProperty("DateModified") != null)
                {
                    entity.GetType().GetProperty("DateModified").SetValue(entity, DateTime.UtcNow);
                }
                if (entity.GetType().GetProperty("Active") != null)
                {
                    entity.GetType().GetProperty("Active").SetValue(entity, true);
                }
                Context.Entry(existing).CurrentValues.SetValues(entity);
            }

        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).AsEnumerable();
        }

        public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).Any();
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).AnyAsync();
        }

        public IQueryable<TEntity> FindComplex(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return query.Where(predicate);
        }

        public List<TEntity> FindComplex(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            foreach (var include in includes)
                query = query.Include(include);

            return query.Where(predicate).ToList();
        }

        //public ListPaginated<TEntity> FindComplexPaginated(Expression<Func<TEntity, bool>> predicate,int pageNumber, params string[] includes)
        //{
        //    IQueryable<TEntity> query = Context.Set<TEntity>();
        //    if (!string.IsNullOrEmpty(includes[0]))
        //    {
        //        foreach (var include in includes)
        //            query = query.Include(include);

        //    }

        //    var items = query.Where(predicate)
        //                      .Skip((pageNumber - 1) * _pageSize)
        //                      .Take(_pageSize)
        //                      .ToList();

        //    var totalCount = Context.Set<TEntity>().Where(predicate).Count();

        //    var obj = new ListPaginated<TEntity>
        //    {
        //        Items = items,
        //        PageSize = _pageSize,
        //        CurrentPage = pageNumber,
        //        TotalCount = totalCount
        //    };

        //    return obj;


        //}

        //public ListPaginated<TEntity> Find(Expression<Func<TEntity, bool>> predicate, int pageNumber)
        //{
        //    var items = Context.Set<TEntity>().Where(predicate)
        //                       .Skip((pageNumber - 1) * _pageSize)
        //                       .Take(_pageSize)
        //                       .ToList();

        //    var totalCount = Context.Set<TEntity>().Where(predicate).Count();

        //    var obj = new ListPaginated<TEntity>
        //    {
        //        Items = items,
        //        PageSize = _pageSize,
        //        CurrentPage = pageNumber,
        //        TotalCount = totalCount
        //    };

        //    return obj;
        //}

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public void Remove(int id)
        {
            var dbItem = Context.Set<TEntity>().Find(id);
            if(dbItem != null)
            {
                if (dbItem.GetType().GetProperty("DateModified") != null)
                {
                    dbItem.GetType().GetProperty("DateModified").SetValue(dbItem, DateTime.UtcNow);
                }
                if (dbItem.GetType().GetProperty("Active") != null)
                {
                    dbItem.GetType().GetProperty("Active").SetValue(dbItem, false);
                }
            }
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        public int Complete()
        {
            return Context.SaveChanges();
        }

    }
}
