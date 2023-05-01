using EMart.Core;
using EMart.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace EMart.Infrastructure.Repository
{
    public class Repository<TObject> : IRepository<TObject> where TObject : class
    {

        //protected QuoteStarDB Context;
        protected DatabaseContext Context = null;

        public Repository(IUnitOfWork uow)
        {
            Context = uow.dbContext;
            //this.Context.Database.GetDbConnection().ConnectionString = EncryptDecrypt.UpdateConnectionStringPassword_Decrypt(this.Context.Database.GetDbConnection().ConnectionString);
        }

        protected DbSet<TObject> DbSet
        {
            get
            {
                return Context.Set<TObject>();
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public virtual IQueryable<TObject> All()
        {
            return DbSet.AsQueryable();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable<TObject>();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter, int pageNumber, int pageSize, out int totalRecords, out int totalPages)
        {
            int skipCount = (pageSize * (pageNumber - 1));
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            totalRecords = _resetSet.Count();
            totalPages = totalRecords == 0 ? 1 : (((totalRecords - 1) / pageSize) + 1);
            _resetSet = skipCount == 0 ? _resetSet.Take(pageSize) : _resetSet.Skip(skipCount).Take(pageSize);
            return _resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public virtual TObject FindOne(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TObject FindOne(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual Task<TObject> FindOneAsync(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual IQueryable<TObject> Find(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public virtual async Task<ICollection<TObject>> FindAsync(Expression<Func<TObject, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public virtual IQueryable<TObject> FindAsNoTracking(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.AsNoTracking().Where(predicate);
        }

        public virtual TObject Create(TObject TObject)
        {
            HandleError_IDENTITY_INSERT(TObject);

            DbSet.Add(TObject);
            return TObject;
        }

        /// <summary>
        /// Handle below exception
        /// SqlException: Cannot insert explicit value for identity column in table 'TABLE_NAME' when IDENTITY_INSERT is set to OFF.
        /// </summary>
        private void HandleError_IDENTITY_INSERT(TObject TObject)
        {
            try
            {
                var id_prop = TObject.GetType().GetProperties().FirstOrDefault(IsDatabaseGeneratedId);
                if (id_prop != null)
                {
                    id_prop.SetValue(TObject, 0);
                }
            }
            catch (Exception ex) { }
        }

        static bool IsDatabaseGeneratedId(PropertyInfo propertyInfo)
        {
            return propertyInfo
                .GetCustomAttributes(typeof(DatabaseGeneratedAttribute))
                .Cast<DatabaseGeneratedAttribute>()
                .Any(a => a.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity);
        }

        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public virtual int Counts(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate);
        }

        public virtual int Delete(TObject TObject)
        {
            DbSet.Remove(TObject);
            return 0;
        }

        public virtual int Update(TObject TObject)
        {
            var entry = Context.Entry(TObject);
            DbSet.Attach(TObject);
            entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return 0;
        }

        public virtual int Delete(Expression<Func<TObject, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);
            return 0;
        }


        public virtual void SetValues(object DestinationValue, object SourceValue)
        {
            Context.Entry(DestinationValue).CurrentValues.SetValues(SourceValue);
        }

        public virtual void Reload(TObject TObject)
        {
            Context.Entry(TObject).Reload();
        }

        public virtual void ReloadReference(TObject TObject, string refProperty)
        {
            Context.Entry(TObject).Reference(refProperty).Load();
        }

        //public static virtual DbDataReader GetExecuteReader(this DbCommand dbCmd)
        //{
        //    DbDataReader dbreader = dbCmd.ExecuteReader();
        //    return dbreader;
        //}

        public void UpdateModifiedProperty(TObject TObject, string PropertyName)
        {
            Context.Entry(TObject).Property(PropertyName).IsModified = true;
        }
        public void UpdateModifiedProperties(TObject TObject, List<string> PropertyNames)
        {
            foreach (var property in PropertyNames)
                Context.Entry(TObject).Property(property).IsModified = true;
        }
        public virtual bool Any(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public virtual decimal? Max(Expression<Func<TObject, decimal?>> predicate)
        {
            return DbSet.Max(predicate);
        }

        public IEnumerable<TObject> DeleteMany(List<TObject> t)
        {
            DbSet.RemoveRange(t);
            return null;
        }

        public TObject FindOneAsNoTracking(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.AsNoTracking().FirstOrDefault(predicate);
        }
        public virtual void CreateMany(IEnumerable<TObject> TObject)
        {
            DbSet.AddRange(TObject);
        }
    }
}
