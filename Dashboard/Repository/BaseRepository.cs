using DashboardSite.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DashboardSite.Model;

namespace DashboardSite.Repository
{
    //public abstract class BaseRepository<TEntity> : IRepository<TEntity>  where TEntity : class, new()
    public abstract class BaseRepository<TEntity> where TEntity : class, new()
    {
        private DbContext _context;
        private DbSet<TEntity> dbSet;
        private IQueryable<TEntity> dbQueryRoot;
        private UnitOfWork _unitOfWork;

        //public BaseRepository(IUnitOfWork unitOfWork)
        public BaseRepository(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._context = unitOfWork.Context;
            this.dbSet = _context.Set<TEntity>();
            this.dbQueryRoot = dbSet.AsQueryable();
        }

        //protected IUnitOfWork UnitOfWork
        protected UnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        protected DbContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// entity framework monitors context in memory for changes
        /// it does not aware of changes from database
        /// needs to reload to sync with database values in some cases
        /// </summary>
        /// <param name="entity"></param>
        protected TEntity GetByIDFresh(int id)
        {
            var entity = GetByIDFast(id);
            _context.Entry(entity).Reload();
            return entity;
        }

        protected virtual IEnumerable<TEntity> GetAll()
        {
            //IQueryable<TEntity> query = dbSet;            
            //return query;
            return dbSet;
        }

        protected virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return dbQueryRoot.Where(predicate);
        }

        protected virtual PaginateSortModel<TEntity> FindWithPageSort(Expression<Func<TEntity, bool>> predicate,
            string strSortBy, SqlOrderByDirecton sortOrder, int pageSize, int pageNum)
        {
            //var query = predicate == null ? GetAll() : Find(predicate);
            var query = Find(predicate);
            return query.SortAndPaginate(strSortBy, sortOrder, pageSize, pageNum);
        }

        protected virtual TEntity GetByID(int id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// dbSet.Find(id) is has performance hit as it calls DetectChanges in context
        /// this is a quicker version
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual TEntity GetByIDFast(int id)
        {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                return GetByID(id);
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        protected virtual void AttachNew(TEntity entity)
        {
            dbSet.Add(entity);
        }

        protected virtual void InsertNew(TEntity entity)
        {
            AttachNew(entity);
            Context.SaveChanges();
        }

        protected virtual void Delete(int id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        protected virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == System.Data.Entity.EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        protected virtual void Update(TEntity entityToUpdate)
        {
            Context.Configuration.AutoDetectChangesEnabled = false;
            dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = System.Data.Entity.EntityState.Modified;
            Context.Configuration.AutoDetectChangesEnabled = true;
        }

        protected virtual void Update(TEntity entityFrom, TEntity entityTo)
        {
            var attachedEntity = _context.Entry(entityTo);
            attachedEntity.CurrentValues.SetValues(entityFrom);
        }

        protected virtual bool Exists<TEntity>(TEntity entity)
        {
            return this.dbSet.Any(e => e.Equals(entity));
        }
    }
}