using EMart.Core;
using EMart.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EMart.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public DatabaseContext dbContext { get; set; }
        public string UserName { get; set; }

        public UnitOfWork(DatabaseContext databaseContext)
        {
            this.dbContext = databaseContext;
        }

        private void AddLastModifiedValues()
        {
            var entities = dbContext.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

            foreach (var item in entities)
            {
                var validationContext = new ValidationContext(item.Entity);
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(item.Entity, validationContext, results, true))
                {
                    var messages = results.Select(r => r.ErrorMessage).ToList().Aggregate((message, nextMessage) => message + ", " + nextMessage);
                    throw new ApplicationException($"Unable to save changes for {item.Entity.GetType().FullName} due to error(s): {messages}");
                }

                if (item == null || item.Entity == null)
                    continue;
                var propertyinfo = item.Entity.GetType().GetProperty("ModifiedDate");
                if (propertyinfo != null)
                {
                    propertyinfo.SetValue(item.Entity, DateTime.UtcNow);
                }
                var propertyinfoModifyBy = item.Entity.GetType().GetProperty("ModifiedBy");
                if (propertyinfoModifyBy != null)
                {
                    if (!string.IsNullOrEmpty(this.UserName))
                        propertyinfoModifyBy.SetValue(item.Entity, this.UserName);
                }
            }
        }

        public void Commit()
        {
            try
            {
                AddLastModifiedValues();
                dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
