using Server_WEB_Programming.Lab2.Dal.Entities;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Server_WEB_Programming.Lab2.Dal.UoW.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Book, int> BookRepository { get; }
        IRepository<Sage, int> SageRepository { get; }
        IRepository<BookOrder, int> BookOrderRepository { get; }

        Task<bool> SaveAsync();
    }
}
