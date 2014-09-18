using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace iPreo.Bigdough.Utilities
{
    /// <summary>
    /// Data-related helpers
    /// </summary>
    static public class DataUtils
    {
        /// <summary>
        /// compare 2 nullable value
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns>
        /// <para>null == null</para>
        /// <para>Value != null</para>
        /// </returns>
        public static bool NullableEquals<T>(Nullable<T> a, Nullable<T>b)
            where T : struct, IEquatable<T>
        {
            if (!a.HasValue && !b.HasValue)
                return true;
            if (a.HasValue && b.HasValue)
            {
                return a.Value.Equals(b.Value); 
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Check if DataSet is emptry or null
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns>true if empty or null</returns>
        static public bool DataSetIsEmptyOrNull(DataSet ds)
        {
            return ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0;
        }

        /// <summary>
        /// Check if DataSet has data
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        static public bool DataSetIsEmpty(DataSet ds)
        {
            if (ds == null)
                throw new ArgumentNullException("ds");

            return ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0;
        }

        /// <summary>
        /// Get number of rows in table from DataSet
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="tableIndex">index of table in DataSet table collection</param>
        /// <returns>Row count or 0 if DataSet empty or not contains table with given index</returns>
        public static int TableRowCount (DataSet ds, int tableIndex)
        {
            if (ds == null || tableIndex >= ds.Tables.Count)
                return 0;
            return ds.Tables[tableIndex].Rows.Count;
        }

        /// <summary>
        /// Lazy create instance
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="item">item ti check</param>
        /// <returns>Object instance</returns>
        static public T LazyCreate<T>(ref T item)
            where T: class, new()
        {
            if (item == null)
                item = new T();
            return item;
        }
        
        /// <summary>
        /// Lazy create instance
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="item">item to check</param>
        /// <param name="creator">Object creator delegate</param>
        /// <returns>Object instance</returns>
        static public T LazyCreate<T>(ref T item, Func<T> creator)
            where T: class 
        {
            if (creator == null)
                throw new ArgumentNullException("creator");

            if (item == null)
            {
                item = creator();
            }
            return item;
        }
        
        public enum PrepareDatasetMode
        {
            contact_list_names,
            contact_accountlist_names,
            contact_all_list_names,
            contact_fund_names,

            company_list_names,
            fund_list_names,
            account_list_names,
            security_list_names
        }

        public static void PrepareDatasetToLoadKeys(DataSet ds, PrepareDatasetMode mode)
        {
            if (mode == PrepareDatasetMode.contact_all_list_names)
            {
                PrepareDataset(ds, PrepareDatasetMode.contact_list_names, false);
                PrepareDataset(ds, PrepareDatasetMode.contact_accountlist_names, false);
            }
            else
            {
                PrepareDataset(ds, mode, false);
            }
        }

        public static void PrepareDatasetToLoadPage(DataSet ds, PrepareDatasetMode mode)
        {
            if (mode == PrepareDatasetMode.contact_all_list_names)
            {
                PrepareDataset(ds, PrepareDatasetMode.contact_list_names, true);
                PrepareDataset(ds, PrepareDatasetMode.contact_accountlist_names, true);
            }
            else
            {
                PrepareDataset(ds, mode, true);
            }
        }

        private static void PrepareDataset(DataSet ds, PrepareDatasetMode mode, bool toLoadPage)
        {
            if (ds == null) throw new ArgumentNullException("ds");
            if(Enum.IsDefined(typeof(PrepareDatasetMode), mode) == false)
            {
                throw new ArgumentOutOfRangeException("mode");
            }

            string sEntityPKColumnName = null;
            string sEntityValueColumnName = null;
            string sEntityToLookUpTable_PKColumnName = null;
            string sEntityToLookUpTable_FKColumnName = null;
            string sLookUpTable_PKColumnName = null;
            string sLookUpTable_ValueColumnName = null;

            if (mode == PrepareDatasetMode.contact_list_names)
            {
                if (toLoadPage)
                {
                    sEntityPKColumnName = "contact_id";
                    sEntityValueColumnName = "list_name";
                }
                sEntityToLookUpTable_PKColumnName = "id";
                sEntityToLookUpTable_FKColumnName = "list_id";
                sLookUpTable_PKColumnName = "list_id";
                sLookUpTable_ValueColumnName = "list_nm";
            }
            else if (mode == PrepareDatasetMode.contact_accountlist_names)
            {
                if (toLoadPage)
                {
                    sEntityPKColumnName = "contact_id";
                    sEntityValueColumnName = "AccountListName";
                }
                sEntityToLookUpTable_PKColumnName = "id";
                sEntityToLookUpTable_FKColumnName = "list_id";
                sLookUpTable_PKColumnName = "list_id";
                sLookUpTable_ValueColumnName = "list_nm";
            }
            else if (mode == PrepareDatasetMode.contact_fund_names)
            {
                if (toLoadPage)
                {
                    // Page loading is not defined because funds are loaded through load strategy "ContactFund"
                    sEntityPKColumnName = "";
                    sEntityValueColumnName = "";
                }

                sEntityToLookUpTable_PKColumnName = "id";
                sEntityToLookUpTable_FKColumnName = "fund_id";
                sLookUpTable_PKColumnName = "fund_id";
                sLookUpTable_ValueColumnName = "fund_nm";
            }
            else if (mode == PrepareDatasetMode.company_list_names)
            {
                if (toLoadPage)
                {
                    sEntityPKColumnName = "company_id";
                    sEntityValueColumnName = "list_name";
                }
                sEntityToLookUpTable_PKColumnName = "id";
                sEntityToLookUpTable_FKColumnName = "list_id";
                sLookUpTable_PKColumnName = "list_id";
                sLookUpTable_ValueColumnName = "list_nm";
            }
            else if (mode == PrepareDatasetMode.fund_list_names)
            {
                if (toLoadPage)
                {
                    sEntityPKColumnName = "fund_id";
                    sEntityValueColumnName = "list_name";
                }
                sEntityToLookUpTable_PKColumnName = "id";
                sEntityToLookUpTable_FKColumnName = "list_id";
                sLookUpTable_PKColumnName = "list_id";
                sLookUpTable_ValueColumnName = "list_nm";
            }
            else if (mode == PrepareDatasetMode.account_list_names)
            {
                if (toLoadPage)
                {
                    sEntityPKColumnName = "account_id";
                    sEntityValueColumnName = "list_name";
                }
                sEntityToLookUpTable_PKColumnName = "id";
                sEntityToLookUpTable_FKColumnName = "list_id";
                sLookUpTable_PKColumnName = "list_id";
                sLookUpTable_ValueColumnName = "list_nm";
            }
            else if (mode == PrepareDatasetMode.security_list_names)
            {
                if (toLoadPage)
                {
                    sEntityPKColumnName = "security_id";
                    sEntityValueColumnName = "list_name";
                }
                sEntityToLookUpTable_PKColumnName = "id";
                sEntityToLookUpTable_FKColumnName = "list_id";
                sLookUpTable_PKColumnName = "list_id";
                sLookUpTable_ValueColumnName = "list_nm";
            }
            
            int iEntityToLookUpTable_Index = toLoadPage ? 1 : 0;
            int iLookUpTable_Index = toLoadPage ? 2 : 1;

            DataTable dtEntityToLookUpTable = ds.Tables[iEntityToLookUpTable_Index];
            DataColumn dcEntityToLookUpTable_PKColumn = dtEntityToLookUpTable.Columns[sEntityToLookUpTable_PKColumnName];
            DataColumn dcEntityToLookUpTable_FKColumn = dtEntityToLookUpTable.Columns[sEntityToLookUpTable_FKColumnName];

            DataTable dtLookUpTable = ds.Tables[iLookUpTable_Index];
            DataColumn dcLookUpTable_PKColumn = dtLookUpTable.Columns[sLookUpTable_PKColumnName];
            DataColumn dcLookUpTable_ValueColumn = dtLookUpTable.Columns[sLookUpTable_ValueColumnName];

            IEnumerable<NamesString> enumerator = GetNamesStringEnumerator(
                    dtEntityToLookUpTable, dcEntityToLookUpTable_PKColumn, dcEntityToLookUpTable_FKColumn,
                    dtLookUpTable, dcLookUpTable_PKColumn, dcLookUpTable_ValueColumn);

            if (toLoadPage)
            {
                DataTable dtEntity = ds.Tables[0];
                    DataColumn dcEntity_PKColumn = dtEntity.Columns[sEntityPKColumnName];
                DataColumn dcEntity_ValueColumn = dtEntity.Columns[sEntityValueColumnName];

                try
                {
                    dtEntity.PrimaryKey = new DataColumn[] { dcEntity_PKColumn };
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error setting primary key on table: {0}. Column Name: {1}", dtEntity.TableName, dcEntity_PKColumn), ex);
                }

                foreach (NamesString oListNamesItem in enumerator)
                {
                    DataRow rowEntity = dtEntity.Rows.Find(oListNamesItem.ID);

                    if (rowEntity == null)
                        throw new ApplicationException(string.Format("The entity with ID = {0} was not returned by the first load strategy. But the list names string was built for it. Maybe this entity was marked as deleted.", oListNamesItem.ID));

                    rowEntity[dcEntity_ValueColumn] = oListNamesItem.ListNames;
                }

                ds.Tables.Remove(dtEntityToLookUpTable);
                ds.Tables.Remove(dtLookUpTable);

                dtEntity.PrimaryKey = null;
            }
            else
            {
                DataTable dtResultTable = new DataTable();
                dtResultTable.Columns.Add("id", typeof(Int32));
                dtResultTable.Columns.Add("SortColumn", typeof(string));

                foreach (NamesString oListNamesItem in enumerator)
                {
                    dtResultTable.Rows.Add(oListNamesItem.ID, oListNamesItem.ListNames);
                }

                ds.Tables.Remove(dtEntityToLookUpTable);
                ds.Tables.Remove(dtLookUpTable);
                ds.Tables.Add(dtResultTable);
            }
        }

        private struct NamesString
        {
            public NamesString(int iID, string sListNames)
            {
                ID = iID;
                ListNames = sListNames;
            }

            public readonly int ID;
            public readonly string ListNames;
        }

        private static IEnumerable<NamesString> GetNamesStringEnumerator(
            DataTable dtEntityToLookUpTable, DataColumn dcEntityToLookUpTable_PKColumn, DataColumn dcEntityToLookUpTable_FKColumn
          , DataTable dtLookUpTable, DataColumn dcLookUpTable_PKColumn, DataColumn dcLookUpTable_ValueColumn)
        {
            if (dtEntityToLookUpTable == null || dtEntityToLookUpTable.Rows.Count == 0) yield break;
            if (dtLookUpTable == null || dtLookUpTable.Rows.Count == 0) yield break;

            dtLookUpTable.PrimaryKey = new DataColumn[] { dcLookUpTable_PKColumn }; 

            const int NOT_VALID = -1;
            string SEPARATOR = ", ";
            int current_entity_id = NOT_VALID;
            StringBuilder sb = new StringBuilder();

            foreach (DataRow oEntityLookUpItemRow in dtEntityToLookUpTable.Rows)
            {
                if (current_entity_id != (int)oEntityLookUpItemRow[dcEntityToLookUpTable_PKColumn])
                {
                    if (current_entity_id != NOT_VALID)
                    {
                        yield return createNameString(current_entity_id, sb, SEPARATOR.Length);
                    }

                    current_entity_id = (int)oEntityLookUpItemRow[dcEntityToLookUpTable_PKColumn];
                    sb = new StringBuilder();
                }

                // Create a comma-separated list of list names.
                if (oEntityLookUpItemRow[dcEntityToLookUpTable_FKColumn] != DBNull.Value)
                {
                    int list_id = (int)oEntityLookUpItemRow[dcEntityToLookUpTable_FKColumn];

                    DataRow oLookUpItemRow = dtLookUpTable.Rows.Find(list_id);
                    if (oLookUpItemRow != null)
                    {
                        string sListName = oLookUpItemRow[dcLookUpTable_ValueColumn] as string;

                        if (sListName != null && sListName.Trim().Length > 0)
                        {
                            sb.Append(oLookUpItemRow[dcLookUpTable_ValueColumn]).Append(SEPARATOR);
                        }
                    }
                }
            }

            if (current_entity_id != NOT_VALID)
            {
                yield return createNameString(current_entity_id, sb, SEPARATOR.Length);
            }
        }

        private static NamesString createNameString(int id, StringBuilder sb, int iSeparatorLength)
        {
            int iStartIndex = sb.Length - iSeparatorLength;
            string s = string.Empty;

            if (iStartIndex > 0)
            {
                s = sb.Remove(iStartIndex, iSeparatorLength).ToString();
            }

            return new NamesString(id, s);
        }

        /// <summary>
        /// Load entities from DataTable
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="dt"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static IEnumerable<T> LoadFromDataTable<T>(DataTable dt, Func<DataRow, T> creator)
        {
            if (dt == null) throw new ArgumentNullException("dt");
            if (creator == null) throw new ArgumentNullException("creator");

            if (dt.Rows.Count == 0)
                yield break;

            foreach (DataRow row in dt.Rows)
            {
                yield return creator(row);
            }
        }

        /// <summary>
        /// Load entities from DataSet
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="tableIndex"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static IEnumerable<T> LoadFromDataSet<T>(DataSet ds, int tableIndex, Func<DataRow, T> creator)
        {
            if (ds == null) throw new ArgumentNullException("ds");
            if (ds.Tables.Count <= tableIndex)
                yield break;
            foreach (T entity in LoadFromDataTable(ds.Tables[tableIndex], creator))
            {
                yield return entity;
            }
        }

    }
}