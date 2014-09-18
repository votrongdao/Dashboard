using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.Model
{
    public class PaginateSortModel<T> where T : class, new()
    {
        public static PaginateSortModel<T> CreateInstance()
        {
            return new PaginateSortModel<T>();
        }
        public int TotalRows { get; set; }
        public IEnumerable<T> Entities { get; set; }
        public int PageSize { get; set; }
    }

    public enum SqlOrderByDirecton
    {
        ASC,
        DESC
    }

    public static class PaginateSort
    {
        public static PaginateSortModel<T> SortAndPaginate<T>(this IQueryable<T> query,
                                         string strSortBy,
                                         SqlOrderByDirecton sortOrder,
                                         int pageSize,
                                         int pageNum) where T : class, new()
        {
            int startRecord = (pageNum - 1) * pageSize;
            var pageSortModel = PaginateSortModel<T>.CreateInstance();
            
            IEnumerable<T> list = null;
            if (sortOrder == SqlOrderByDirecton.ASC)
            {
                //list = query.OrderBy(sortBy).Skip(startRecord).Take(pageSize);
                list = query.OrderBy(strSortBy).Skip(startRecord).Take(pageSize);
            }
            else
            {
                //list = query.OrderByDescending(sortBy).Skip(startRecord).Take(pageSize);
                list = query.OrderByDescending(strSortBy).Skip(startRecord).Take(pageSize);
            }
            
            pageSortModel.Entities = list.ToList();
            pageSortModel.TotalRows = query.Count();
            pageSortModel.PageSize = pageSize;
            return pageSortModel;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string property)
        {
            return ApplyOrder<T>(query, property, "OrderBy");
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string property)
        {
            return ApplyOrder<T>(query, property, "OrderByDescending");
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> query, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach(string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                               method => method.Name == methodName
                               && method.IsGenericMethodDefinition
                               && method.GetGenericArguments().Length == 2
                               && method.GetParameters().Length == 2)
                               .MakeGenericMethod(typeof(T), type)
                               .Invoke(null, new object[] { query, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
