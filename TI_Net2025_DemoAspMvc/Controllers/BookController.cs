using Microsoft.AspNetCore.Mvc;
using TI_Net2025_DemoAspMvc.Datas;
using TI_Net2025_DemoAspMvc.Models;

namespace TI_Net2025_DemoAspMvc.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            List<Book> books = [.. FakeDb.Books];
            return View(books);
        }

        [HttpGet("/book/details/{isbn}")]
        public IActionResult Details([FromRoute] string isbn)
        {
            //Book book = null;

            //foreach (Book b in FakeDb.Books)
            //{
            //    if(b.Isbn == isbn)
            //    {
            //        book = b;
            //        break;
            //    }
            //}

            Book book = FakeDb.Books.SingleOrDefault(b => b.Isbn == isbn);

            if (book == null)
            {
                throw new Exception($"Book with isbn {isbn} not found");
            }

            return View(book);
        }

        public IActionResult Add()
        {
            return View(new Book());
        }

        [HttpPost]
        public IActionResult Add([FromForm] Book book)
        {

            FakeDb.Books.Add(book);

            return RedirectToAction("Index", "Book");
        }

        [HttpGet("/book/edit/{isbn}")]
        public IActionResult Edit([FromRoute] string isbn)
        {
            Book book = FakeDb.Books.SingleOrDefault(b => b.Isbn == isbn);

            if (book == null)
            {
                throw new Exception($"Book with isbn {isbn} not found");
            }

            return View(book);
        }

        [HttpPost("/book/edit/{isbn}")]
        public IActionResult Edit([FromRoute] string isbn, [FromForm] Book book)
        {
            Book existing = FakeDb.Books.SingleOrDefault(book => book.Isbn == isbn);

            if (existing == null)
            {
                throw new Exception($"Book with isbn {isbn} not found");
            }

            existing.Isbn = book.Isbn;
            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Release = book.Release;
            existing.Description = book.Description;

            return RedirectToAction("Index", "Book");
        }

        [HttpPost("/book/remove/{isbn}")]
        public IActionResult Remove([FromRoute] string isbn)
        {
            Book book = FakeDb.Books.SingleOrDefault(b => b.Isbn == isbn);

            if (book == null)
            {
                throw new Exception($"Book with isbn {isbn} not found");
            }

            FakeDb.Books.Remove(book);

            return RedirectToAction("Index", "Book");
        }
    }
}
