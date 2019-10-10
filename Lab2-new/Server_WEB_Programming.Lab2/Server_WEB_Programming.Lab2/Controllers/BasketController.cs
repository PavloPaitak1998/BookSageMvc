using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Server_WEB_Programming.Lab2.Dal.Entities;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces;

namespace Server_WEB_Programming.Lab2.Controllers
{
    public class BasketController : Controller
    {
        private readonly IUnitOfWork _uow;

        public BasketController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Basket
        public ActionResult Index()
        {
            if (Session["basket"] == null)
            {
                var basket = new List<BookOrder>();

                Session["basket"] = basket;
            }

            return View();
        }

        public async Task<ActionResult> Buy(int id)
        {
            var book = await _uow.BookRepository.GetFirstOrDefaultAsync(
                           x => x.IdBook == id,
                           include: q => q.Include(x => x.Sages));

            if (Session["basket"] == null)
            {
                var basket = new List<BookOrder>();

                basket.Add(new BookOrder { BookId = id, Book = book, Quantity = 1 });

                Session["basket"] = basket;
            }
            else
            {
                var basket = (List<BookOrder>)Session["basket"];

                var index = IsExist(id);

                if (index != -1)
                {
                    basket[index].Quantity++;
                }
                else
                {
                    basket.Add(new BookOrder { BookId = id, Book = book, Quantity = 1 });
                }

                Session["basket"] = basket;
            }

            return RedirectToAction("Index", "Books");
        }

        public ActionResult Remove(int id)
        {
            var basket = (List<BookOrder>)Session["basket"];
            var index = IsExist(id);

            basket.RemoveAt(index);

            Session["basket"] = basket;

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CompleteOrder()
        {
            var bookOrders = (List<BookOrder>)Session["basket"];

            if (bookOrders.Count > 0)
            {
                foreach (var item in bookOrders)
                {
                    await _uow.BookOrderRepository.CreateAsync(item);
                }

                await _uow.SaveAsync();
                Session["basket"] = new List<BookOrder>();
            }

            return RedirectToAction("Index");
        }

        private int IsExist(int id)
        {
            var basket = (List<BookOrder>)Session["basket"];

            for (int i = 0; i < basket.Count; i++)
            {
                if (basket[i].BookId.Equals(id))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}