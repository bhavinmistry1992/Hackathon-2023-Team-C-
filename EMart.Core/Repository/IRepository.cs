using System.Linq.Expressions;

namespace EMart.Core.Repository
{
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Gets all objects from database
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All();

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <returns></returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets objects from database with filting and paging.
        /// </summary>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns></returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

        /// <summary>
        /// Gets objects from database with filtering and paging.
        /// </summary>
        /// <param name="filter">Specified a filter</param>        
        /// <param name="totalRecords">Returns the total records count of the filter.</param>
        /// <param name="totalPages">Returns the total pages.</param>
        /// <param name="pageNumber">Specified the page number.</param>
        /// <param name="pageSize">Specified the page size</param>
        /// <returns></returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, int pageNumber, int pageSize, out int totalRecords, out int totalPages);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        /// <returns></returns>
        bool Contains(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        /// <returns></returns>
        T FindOne(params object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T FindOne(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find all matched object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find all matched object by specified expression with no tracking for db changes.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        /// <returns></returns>
        T Create(T t);

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>
        int Delete(T t);

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
        /// <returns></returns>
        int Update(T t);
        int Count { get; }
        void SetValues(object DestinationValue, object SourceValue);
        void Reload(T t);
        void ReloadReference(T t, string refProperty);
        void UpdateModifiedProperty(T t, string PropertyName);
        void UpdateModifiedProperties(T t, List<string> PropertyNames);
        bool Any(Expression<Func<T, bool>> predicate);
        Task<T> FindOneAsync(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> predicate);
        int Counts(Expression<Func<T, bool>> predicate);
        decimal? Max(Expression<Func<T, decimal?>> predicate);
        IEnumerable<T> DeleteMany(List<T> t);
        T FindOneAsNoTracking(Expression<Func<T, bool>> predicate);
        void CreateMany(IEnumerable<T> t);
    }
}
