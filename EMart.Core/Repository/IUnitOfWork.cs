namespace EMart.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        DatabaseContext dbContext { get; set; }
        string UserName { get; set; }
        void Commit();
    }
}
