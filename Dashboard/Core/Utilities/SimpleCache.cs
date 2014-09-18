using System;
using System.Collections.Generic;


namespace DashboardSite.Core.Utilities
{
    /// <summary>
    /// Simple cache.
    /// </summary>
    /// <typeparam name="TKey">key</typeparam>
    /// <typeparam name="TValue">item</typeparam>
    /// <remarks>   
    /// Cache provides no support for item expiration
    /// All members are thread-safe.
    ///</remarks>
    public class SimpleCache<TKey, TValue>
        where TValue : class
    {
        #region Private Fields

        private readonly object m_lockObj;
        private readonly Dictionary<TKey, TValue> m_cache;

        #endregion Private Fields

        #region Constructor

        public SimpleCache()
        {
            m_lockObj = new object();
            m_cache = new Dictionary<TKey, TValue>();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Get item from cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Item or null if not found</returns>
        public TValue Get(TKey key)
        {
            lock (m_lockObj)
            {
                TValue item;
                return m_cache.TryGetValue(key, out item) ? item : null;
            }
        }

        /// <summary>
        /// Add item to cache
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="item">item</param>
        public void Add(TKey key, TValue item)
        {
            lock (m_lockObj)
            {
                m_cache[key] = item;
            }
        }

        #endregion Public Methods

        #region Protected and Override Methods

        #endregion Protected and Override Methods

        #region Private Methods

        #endregion Private Methods
    }
}