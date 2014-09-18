using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DashboardSite.Core;
using System.Data.Entity;
using DashboardSite.Repository.Interface;
using System.Linq;
using System.Linq.Expressions;

namespace DashboardSite.Repository
{
    abstract public class BaseLookupCache<TEntity> : BaseRepository<TEntity> where TEntity : class, new()
    {
        static protected Dictionary<int, TEntity> m_oLookupDictById = new Dictionary<int, TEntity>();
        static protected Dictionary<string, TEntity> m_oLookupDictByCd = new Dictionary<string, TEntity>();

        protected virtual string IdColName { get { throw new NotImplementedException(); } }
        protected virtual string CdColName { get { throw new NotImplementedException(); } }
        protected virtual string ValColName { get { throw new NotImplementedException(); } }

        public BaseLookupCache(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected virtual List<TEntity> GetLookupList() 
        {
            if (m_oLookupDictById.Count == 0)
            {
                var allEntities = this.GetAll();
                foreach (var item in allEntities)
                {
                    int idVal = int.Parse(item.GetType().GetProperty(IdColName).GetValue(item, null).ToString());
                    m_oLookupDictById.Add(idVal, item);
                }
            }
            var entityList = new List<TEntity>();
            foreach(var item in m_oLookupDictById.Keys)
            {
                entityList.Add(m_oLookupDictById[item]);
            }
            return entityList;
        }

        protected virtual TEntity GetById(int id)
        {
            // if not cache yet, load lookup into dictionary
            if(m_oLookupDictById.Count == 0)
            {
                var allEntities = this.GetAll();
                foreach(var item in allEntities)
                {
                    int idVal = int.Parse(item.GetType().GetProperty(IdColName).GetValue(item, null).ToString());
                    m_oLookupDictById.Add(idVal, item);
                }
            }
            return m_oLookupDictById[id];
        }

        protected virtual TEntity GetByCd(string code)
        {            
            // if not cache yet, load lookup into dictionary
            if (m_oLookupDictByCd.Count == 0)
            {
                var allEntities = this.GetAll();
                foreach (var item in allEntities)
                {
                    string cdVal = item.GetType().GetProperty(CdColName).GetValue(item, null).ToString();
                    m_oLookupDictByCd.Add(cdVal, item);
                }
            }
            return m_oLookupDictByCd[code];
        }
    }
}

