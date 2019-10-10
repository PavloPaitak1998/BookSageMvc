using AutoMapper;
using Server_WEB_Programming.Lab2.Dal.DataBase;
using Server_WEB_Programming.Lab2.Dal.Entities;
using Server_WEB_Programming.Lab2.Dal.UoW.Implementations.Repositories;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces.Repositories;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Server_WEB_Programming.Lab2.Dal.UoW.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;

        private IRepository<Book, int> _bookRepository;
        private IRepository<Sage, int> _sageRepository;
        private IRepository<BookOrder, int> _bookOrderRepository;

        public UnitOfWork(IMapper mapper)
        {
            _context = new SageBookDbContext();
            _mapper = mapper;
        }

        public IRepository<Book, int> BookRepository =>
            _bookRepository ?? (_bookRepository = new Repository<Book, int>(_context, _mapper));

        public IRepository<Sage, int> SageRepository =>
            _sageRepository ?? (_sageRepository = new Repository<Sage, int>(_context, _mapper));

        public IRepository<BookOrder, int> BookOrderRepository =>
            _bookOrderRepository ?? (_bookOrderRepository = new Repository<BookOrder, int>(_context, _mapper));

        public async Task<bool> SaveAsync()
        {
            try
            {
                var changes = _context.ChangeTracker.Entries().Count(
                    p => p.State == EntityState.Modified || p.State == EntityState.Deleted
                                                         || p.State == EntityState.Added);
                if (changes == 0) return true;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        #region IDisposable Support

        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // DbSet?.Local?.Clear();
                    _context?.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.


                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AbstractRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
