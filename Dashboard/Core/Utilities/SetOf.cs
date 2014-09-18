using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DashboardSite.Core.Utilities
{
    /// <summary>
    /// Generic SET
    /// It is a collection which allows only one instance of object to be added to
    /// with additional features and sets operations
    /// </summary>
    /// <typeparam name="T">Any</typeparam>
    public class SetOf<T> : IList<T>
    {
        #region Private Fields and Construction

        private readonly List<T> m_oInnerList = new List<T>();

        public SetOf()
        {
            
        }

        public SetOf(IEnumerable<T> oItems)
        {
            AddRange(oItems);
        }

        #endregion

        #region List<T> members

        public IEnumerator<T> GetEnumerator()
        {
            return m_oInnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (!m_oInnerList.Contains(item)) m_oInnerList.Add(item);
        }

        public void Clear()
        {
            m_oInnerList.Clear();
        }

        public bool Contains(T item)
        {
            return m_oInnerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_oInnerList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return m_oInnerList.Remove(item);
        }

        public int Count
        {
            get { return m_oInnerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            return m_oInnerList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Inserting by index is not allowed");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Removing by index is not allowed");
        }

        public T this[int index]
        {
            get { return m_oInnerList[index]; }
            set { throw new NotSupportedException("Changing of values by index is not allowed"); }
        }

        public void AddRange(IEnumerable<T> oItems)
        {
            foreach (T oItem in oItems) Add(oItem);
        }

        public T[] ToArray()
        {
            return this.m_oInnerList.ToArray();
        }

        #endregion

        #region Operations on Sets

        public SetOf<T> UnionWith(SetOf<T> oSet)
        {
            SetOf<T> oResult = new SetOf<T>(this);
            oResult.AddRange(oSet);
            return oResult;
        }

        public SetOf<T> IntersectWith(SetOf<T> oSet)
        {
            SetOf<T> oResult = new SetOf<T>(this);
            foreach (T oItem in this)
            {
                if (!oSet.Contains(oItem)) oResult.Remove(oItem);
            }
            return oResult;
        }

        public SetOf<T> ExceptWith(SetOf<T> oSet)
        {
            SetOf<T> oResult = new SetOf<T>(this);
            foreach (T oItem in oSet)
            {
                if (Contains(oItem)) oResult.Remove(oItem);
            }
            return oResult;
        }

        #endregion

        #region Special features for Enums

        /// <summary>
        /// Enumerates all values in enum type tEnum and adds to the set only those
        /// which are marked by attribute ATTR which corresponds to oCriteria criteria
        /// </summary>
        /// <param name="oCriteria">Object of the Attribute type where the only fileds 
        /// which should be correspond as criteria should be set</param>
        protected void AddItemsFromEnumByCriteria(SetGroupAttribute oCriteria)
        {
            AddItemsByGenericCriteria<SetGroupAttribute, SetGroupAttribute>(oCriteria);
        }

        protected void AddItemsFromEnumByFlags(Enum eEnumValue)
        {
            Int64 iSearchValue = Convert.ToInt64(eEnumValue);
            AddItemsByGenericCriteria<SetFlagsAttribute, Int64>(iSearchValue);
        }

        protected void AddItemsByGenericCriteria<ATTR, CRIT>(CRIT oCriteria) where ATTR : Attribute, IComparable<CRIT>
        {
            Type tEnum = typeof(T);
            if (!tEnum.IsEnum) throw new NotSupportedException("This method is allowed only for sets based on enums");
            foreach (FieldInfo oField in tEnum.GetFields())
            {
                object[] oAttributes = oField.GetCustomAttributes(typeof(ATTR), false);
                if (oAttributes != null && oAttributes.Length > 0)
                {
                    ATTR oAttr = oAttributes[0] as ATTR;
                    if (oAttr != null && oAttr.CompareTo(oCriteria) == 0)
                    {
                        Add((T)oField.GetRawConstantValue());
                    }
                }
            }
        }

        #endregion

        #region Static Methods

        public static SetOf<T> Union(params SetOf<T>[] oSets)
        {
            SetOf<T> oResult = new SetOf<T>();
            foreach (SetOf<T> oSet in oSets)
            {
                oResult = oResult.UnionWith(oSet);
            }
            return oResult;
        }

        public static SetOf<T> Intersect(params SetOf<T>[] oSets)
        {
            SetOf<T> oResult = null;
            foreach (SetOf<T> oSet in oSets)
            {
                if (oResult == null) oResult = new SetOf<T>(oSet);
                oResult = oResult.IntersectWith(oSet);
            }
            return oResult;
        }

        public static SetOf<T> Except(SetOf<T> oBaseSet, SetOf<T> oSetToExcept)
        {
            return oBaseSet.ExceptWith(oSetToExcept);
        }

        #endregion
    }

    #region Attributes for Grouping

    /// <summary>
    /// Special attribute to mark Enum values for grouping by complex criteria
    /// </summary>
    public abstract class SetGroupAttribute : Attribute, IComparable<SetGroupAttribute>
    {
        private readonly Dictionary<string, object> m_Dimensions = new Dictionary<string, object>();
        protected object this[string sKey]
        {
            get
            {
                object oValue;
                if (!m_Dimensions.TryGetValue(sKey, out oValue)) return null;
                return oValue;
            }
            set
            {
                m_Dimensions.Remove(sKey);
                m_Dimensions.Add(sKey, value);
            }
        }

        /// <summary>
        /// Returns null if significant (not null) properties of 
        /// OTHER are equal with the properties on this
        /// </summary>
        /// <param name="criteria">Another attribute object</param>
        /// <returns>0 - when equal, >0 - count of differences</returns>
        public int CompareTo(SetGroupAttribute criteria)
        {
            int iDiffCount = 0;
            foreach (KeyValuePair<string, object> oPair in criteria.m_Dimensions)
            {
                object thisValue;
                if (!m_Dimensions.TryGetValue(oPair.Key, out thisValue)) iDiffCount++;
                else if (thisValue != oPair.Value) iDiffCount++;
            }
            return iDiffCount;
        }
    }

    /// <summary>
    /// Special attribute to mark Enum values for grouping by flags
    /// </summary>
    public abstract class SetFlagsAttribute : Attribute, IComparable<Int64>
    {
        protected abstract Int64 GetFlagsIntegerValue();

        public int CompareTo(Int64 otherFlagsValue)
        {
            long thisFlagsValue = GetFlagsIntegerValue();
            if ((otherFlagsValue == 0) || 
                (thisFlagsValue == otherFlagsValue) || 
                ((thisFlagsValue & otherFlagsValue) > 0)) return 0;
            return 1; // not equal
        }
    }

    #endregion

    public class StringValueAttribute : Attribute
    {
        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set { _value = value;}
        }
        
        public StringValueAttribute(string value)
        {
            _value = value;
        }
    }
}
