using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Server_WEB_Programming.Lab2.Dal.DataBase;
using Server_WEB_Programming.Lab2.Dal.Entities;
using System.Web;

using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces;
using Server_WEB_Programming.Lab2.ViewModels;

namespace Server_WEB_Programming.Lab2.Controllers
{
    public class SagesController : Controller
    {
        private readonly IUnitOfWork _uow;

        public SagesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Sages
        public async Task<ActionResult> Index()
        {
            return View(await _uow.SageRepository.GetAllAsync(null, null, q => q.Include(x => x.Books)));
        }

        // GET: Sages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sage = await _uow.SageRepository
                           .GetFirstOrDefaultAsync(
                               x => x.IdSage.Equals(id.Value),
                               null,
                               q => q.Include(x => x.Books),
                               disableTracking: false);

            if (sage == null)
            {
                return HttpNotFound();
            }

            return View(sage);
        }

        [Authorize(Roles = "admin")]
        // GET: Sages/Create
        public async Task<ActionResult> Create()
        {
            var sageViewModel = new SageViewModel();

            var sages = await _uow.SageRepository.GetAllAsync();

            sageViewModel.AllBooks = sages
                .Select(x => new SelectListItem
                {
                    Value = x.IdSage.ToString(),
                    Text = x.Name
                })
                .ToList();

            return View(sageViewModel);
        }

        [Authorize(Roles = "admin")]

        // POST: Sages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SageViewModel sageViewModel, HttpPostedFileBase photo1)
        {
            if (ModelState.IsValid)
            {
                if (photo1 != null)
                {
                    sageViewModel.Sage.Photo = new byte[photo1.ContentLength];
                    photo1.InputStream.Read(sageViewModel.Sage.Photo, 0, photo1.ContentLength);
                }

                var selectedBooks = new HashSet<int>(sageViewModel.SelectedBooks);

                var books = await _uow.BookRepository.GetAllAsync(filter: x => selectedBooks.Contains(x.IdBook), disableTracking: false);

                sageViewModel.Sage.Books = books.ToList();

                await _uow.SageRepository.CreateAsync(sageViewModel.Sage);
                await _uow.SaveAsync();
                return RedirectToAction("Index");
            }

            return View(sageViewModel);
        }

        [Authorize(Roles = "admin")]
        // GET: Sages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sageViewModel = new SageViewModel
            {
                Sage = await _uow.SageRepository
                    .GetFirstOrDefaultAsync(
                        x => x.IdSage.Equals(id.Value),
                        null,
                        q => q.Include(x => x.Books),
                        disableTracking: false)
            };

            if (sageViewModel.Sage == null)
            {
                return HttpNotFound();
            }

            var books = await _uow.BookRepository.GetAllAsync(disableTracking: false);

            sageViewModel.AllBooks = books
                .Select(x => new SelectListItem
                {
                    Value = x.IdBook.ToString(),
                    Text = x.Name
                })
                .ToList();


            return View(sageViewModel);
        }

        [Authorize(Roles = "admin")]

        // POST: Sages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SageViewModel sageViewModel, HttpPostedFileBase photo1)
        {
            if (ModelState.IsValid)
            {
                if (photo1 != null)
                {
                    sageViewModel.Sage.Photo = new byte[photo1.ContentLength];
                    photo1.InputStream.Read(sageViewModel.Sage.Photo, 0, photo1.ContentLength);
                }

                var sageToUpdate = await _uow.SageRepository
                    .GetFirstOrDefaultAsync(
                        x => x.IdSage.Equals(sageViewModel.Sage.IdSage),
                        null,
                        q => q.Include(x => x.Books),
                        disableTracking: false);

                if (sageToUpdate == null)
                {
                    return HttpNotFound();
                }

                sageToUpdate.Name = sageViewModel.Sage.Name;
                sageToUpdate.Photo = sageViewModel.Sage.Photo;
                sageToUpdate.Age = sageViewModel.Sage.Age;
                sageToUpdate.City = sageViewModel.Sage.City;

                var selectedBooks = new HashSet<int>(sageViewModel.SelectedBooks);
                var sageBooks = new HashSet<int>(sageToUpdate.Books.Select(c => c.IdBook));

                var books = await _uow.BookRepository.GetAllAsync(disableTracking: false);

                foreach (var book in books)
                {
                    if (selectedBooks.Contains(book.IdBook))
                    {
                        if (!sageBooks.Contains(book.IdBook))
                        {
                            sageToUpdate.Books.Add(book);
                        }
                    }
                    else
                    {
                        if (sageBooks.Contains(book.IdBook))
                        {
                            sageToUpdate.Books.Remove(book);
                        }
                    }
                }

                await _uow.SageRepository.UpdateAsync(sageToUpdate);
                await _uow.SaveAsync();

                return RedirectToAction("Index");
            }

            return View(sageViewModel);
        }

        [Authorize(Roles = "admin")]

        // GET: Sages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Sage sage = await _uow.SageRepository.GetByIdAsync(id.Value);

            if (sage == null)
            {
                return HttpNotFound();
            }

            return View(sage);
        }

        [Authorize(Roles = "admin")]

        // POST: Sages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _uow.SageRepository.DeleteAsync(id);
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
