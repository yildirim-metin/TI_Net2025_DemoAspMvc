using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using TI_Net2025_DemoAspMvc.Datas;
using TI_Net2025_DemoAspMvc.Mappers;
using TI_Net2025_DemoAspMvc.Models.Dtos;
using TI_Net2025_DemoAspMvc.Models.Entities;

namespace TI_Net2025_DemoAspMvc.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            List<Book> books = [.. FakeDb.Books];

            List<BookIndexDto> dtos = books
                .Select(book => book.ToBookIndexDto())
                .ToList();

            return View(dtos);
        }

        [HttpGet("/book/details/{isbn}")]
        public IActionResult Details([FromRoute] string isbn)
        {

            Book book = FakeDb.Books.SingleOrDefault(b => b.Isbn == isbn);

            if (book == null)
            {
                throw new Exception($"Book with isbn {isbn} not found");
            }

            return View(book.ToBookDetailDto());
        }

        public IActionResult Add()
        {
            return View(new BookFormDto());
        }

        [HttpPost]
        public IActionResult Add([FromForm] BookFormDto book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            FakeDb.Books.Add(book.ToBook());

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

            return View(book.ToBookFormDto());
        }

        [HttpPost("/book/edit/{isbn}")]
        public IActionResult Edit([FromRoute] string isbn, [FromForm] BookFormDto book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            Book existing = FakeDb.Books.SingleOrDefault(book => book.Isbn == isbn);

            if (existing == null)
            {
                throw new Exception($"Book with isbn {isbn} not found");
            }

            existing.Isbn = book.Isbn;
            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Release = (DateTime) book.Release;
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
