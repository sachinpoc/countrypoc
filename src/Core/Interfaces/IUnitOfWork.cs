using MyCleanArchitectureApp.Core.Entities;

namespace MyCleanArchitectureApp.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Country> CountryRepository { get; }
        IRepository<State> StateRepository { get; }
        Task<int> SaveAsync();
    }
}
