using MyCleanArchitectureApp.Core.Entities;
using MyCleanArchitectureApp.Core.Interfaces;
using MyCleanArchitectureApp.Infrastructure.Data;
using MyCleanArchitectureApp.Infrastructure.Repositories;

namespace MyCleanArchitectureApp.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<Country> CountryRepository { get; }
        public IRepository<State> StateRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            CountryRepository = new GenericRepository<Country>(_context);
            StateRepository = new GenericRepository<State>(_context);
        }

        public async Task<int> SaveAsync() 
        {
            return await _context.SaveChangesAsync(); 
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
