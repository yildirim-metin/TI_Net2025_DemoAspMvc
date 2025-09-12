using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TI_Net2025_DemoAspMvc.Datas;
using TI_Net2025_DemoAspMvc.Mappers;
using TI_Net2025_DemoAspMvc.Models.Dtos.Book;
using TI_Net2025_DemoAspMvc.Models.Entities;

namespace TI_Net2025_DemoAspMvc.Controllers
{
    public class BookController : Controller
    {
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=BookDb;Trusted_Connection=True;";

        public IActionResult Index()
        {

            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"SELECT * 
                                    FROM BOOK B 
                                        JOIN AUTHOR A 
                                            ON B.AUTHOR_ID = A.ID";

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Isbn = (string)reader["ISBN"],
                        Title = (string)reader["TITLE"],
                        Description = reader["DESCRIPTION"] == DBNull.Value ? null : (string)reader["DESCRIPTION"],
                        Release = (DateTime)reader["RELEASE"],
                        AuthorId = (int) reader["AUTHOR_ID"],
                        Author = new Author()
                        {
                            Id = (int)reader["AUTHOR_ID"],
                            Firstname = (string) reader["FIRST_NAME"],
                            Lastname = (string) reader["LAST_NAME"],
                        },
                    });
                }

                connection.Close();
            }


            List<BookIndexDto> dtos = books
                .Select(book => book.ToBookIndexDto())
                .ToList();

            return View(dtos);
        }

        [HttpGet("/book/details/{isbn}")]
        public IActionResult Details([FromRoute] string isbn)
        {

            Book book = FakeDb.Books.SingleOrDefault(b => b.Isbn == isbn);
            book.Author = FakeDb.Authors.SingleOrDefault(a => a.Id == book.AuthorId);

            if (book == null)
            {
                throw new Exception($"Book with isbn {isbn} not found");
            }

            return View(book.ToBookDetailDto());
        }

        public IActionResult Add()
        {
            ViewData.Add("Authors", FakeDb.Authors);
            return View(new BookFormDto());
        }

        [HttpPost]
        public IActionResult Add([FromForm] BookFormDto book)
        {
            if (!ModelState.IsValid)
            {
                ViewData.Add("Authors", FakeDb.Authors);
                return View(book);
            }

            if(!FakeDb.Authors.Any(a=> a.Id == book.AuthorId))
            {
                throw new Exception($"Author with AuthorId {book.AuthorId} doesn't exist");
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

            ViewData.Add("Authors", FakeDb.Authors);

            return View(book.ToBookFormDto());
        }

        [HttpPost("/book/edit/{isbn}")]
        public IActionResult Edit([FromRoute] string isbn, [FromForm] BookFormDto book)
        {
            if (!ModelState.IsValid)
            {
                ViewData.Add("Authors", FakeDb.Authors);
                return View(book);
            }

            Book existing = FakeDb.Books.SingleOrDefault(book => book.Isbn == isbn);

            if (existing == null)
            {
                throw new Exception($"Book with isbn {isbn} not found");
            }

            if (!FakeDb.Authors.Any(a => a.Id == book.AuthorId))
            {
                throw new Exception($"Author with AuthorId {book.AuthorId} doesn't exist");
            }

            existing.Isbn = book.Isbn;
            existing.Title = book.Title;
            existing.AuthorId = book.AuthorId;
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
