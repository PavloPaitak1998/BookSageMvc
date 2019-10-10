using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Server_WEB_Programming.Lab2.Dal.DataBase;
using Server_WEB_Programming.Lab2.Dal.Entities;
using Server_WEB_Programming.Lab2.ViewModels;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces;

namespace Server_WEB_Programming.Lab2.Controllers
{
    public class BooksController : Controller
    {
        private readonly IUnitOfWork _uow;

        public BooksController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Books
        public async Task<ActionResult> Index()
        {
            return View(await _uow.BookRepository.GetAllAsync(null, null, q => q.Include(x => x.Sages)));
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Book book = await db.Books.FindAsync(id);
            var book = await _uow.BookRepository
                .GetFirstOrDefaultAsync(
                    x => x.IdBook.Equals(id.Value),
                    null,
                    q => q.Include(x => x.Sages),
                    disableTracking:false);

            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        [Authorize(Roles = "admin")]
        // GET: Books/Create
        public async Task<ActionResult> Create()
        {
            var bookViewModel = new BookViewModel();

            var sages = await _uow.SageRepository.GetAllAsync();

            bookViewModel.AllSages = sages
                .Select(x => new SelectListItem
                {
                    Value = x.IdSage.ToString(),
                    Text = x.Name
                })
                .ToList();

            return View(bookViewModel);
        }

        [Authorize(Roles = "admin")]

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var selectedSages = new HashSet<int>(bookViewModel.SelectedSages);

                var sages = await _uow.SageRepository.GetAllAsync(filter:x => selectedSages.Contains(x.IdSage), disableTracking:false);

                bookViewModel.Book.Sages = sages.ToList();

                await _uow.BookRepository.CreateAsync(bookViewModel.Book);
                await _uow.SaveAsync();
                return RedirectToAction("Index");
            }

            return View(bookViewModel);
        }

        [Authorize(Roles = "admin")]

        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Book book = await db.Books.FindAsync(id);
            var bookViewModel = new BookViewModel
            {
                Book = await _uow.BookRepository
                    .GetFirstOrDefaultAsync(
                        x => x.IdBook.Equals(id.Value),
                        null,
                        q => q.Include(x => x.Sages),
                        disableTracking: false)
            };

            if (bookViewModel.Book == null)
            {
                return HttpNotFound();
            }

            var sages = await _uow.SageRepository.GetAllAsync(disableTracking:false);

            bookViewModel.AllSages = sages
                .Select(x => new SelectListItem
                {
                    Value = x.IdSage.ToString(),
                    Text = x.Name
                })
                .ToList();


            return View(bookViewModel);
        }

        [Authorize(Roles = "admin")]

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var bookToUpdate = await _uow.BookRepository
                    .GetFirstOrDefaultAsync(
                        x => x.IdBook.Equals(bookViewModel.Book.IdBook),
                        null,
                        q => q.Include(x => x.Sages),
                        disableTracking: false);

                if (bookToUpdate == null)
                {
                    return HttpNotFound();
                }

                bookToUpdate.Name = bookViewModel.Book.Name;
                bookToUpdate.Description = bookViewModel.Book.Description;

                var selectedSages = new HashSet<int>(bookViewModel.SelectedSages);
                var bookSages = new HashSet<int>(bookToUpdate.Sages.Select(c => c.IdSage));

                var sages = await _uow.SageRepository.GetAllAsync(disableTracking: false);

                foreach (var sage in sages)
                {
                    if (selectedSages.Contains(sage.IdSage))
                    {
                        if (!bookSages.Contains(sage.IdSage))
                        {
                            bookToUpdate.Sages.Add(sage);
                        }
                    }
                    else
                    {
                        if (bookSages.Contains(sage.IdSage))
                        {
                            bookToUpdate.Sages.Remove(sage);
                        }
                    }
                }
                await _uow.BookRepository.UpdateAsync(bookToUpdate);
                await _uow.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(bookViewModel);
        }

        [Authorize(Roles = "admin")]

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await _uow.BookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        [Authorize(Roles = "admin")]

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _uow.BookRepository.DeleteAsync(id);
            await _uow.SaveAsync();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
