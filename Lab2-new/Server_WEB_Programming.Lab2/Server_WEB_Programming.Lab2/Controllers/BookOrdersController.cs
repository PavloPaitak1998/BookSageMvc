using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces;

namespace Server_WEB_Programming.Lab2.Controllers
{
    [Authorize(Roles = "admin")]
    public class BookOrdersController : Controller
    {
        private readonly IUnitOfWork _uow;

        public BookOrdersController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: BookOrders
        public async Task<ActionResult> Index()
        {
            return View(await _uow.BookOrderRepository.GetAllAsync(include: q => q.Include(x => x.Book.Sages)));
        }
    }
}