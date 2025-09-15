using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TI_Net2025_DemoAspMvc.Datas;
using TI_Net2025_DemoAspMvc.Mappers;
using TI_Net2025_DemoAspMvc.Models.Dtos.Author;
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
                        AuthorId = (int)reader["AUTHOR_ID"],
                        Author = new Author()
                        {
                            Id = (int)reader["AUTHOR_ID"],
                            Firstname = (string)reader["FIRST_NAME"],
                            Lastname = (string)reader["LAST_NAME"],
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

            Book book;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @$"SELECT * 
                                    FROM BOOK B 
                                        JOIN AUTHOR A 
                                            ON B.AUTHOR_ID = A.ID 
                                    WHERE ISBN = @isbn";

                cmd.Parameters.AddWithValue("@isbn", isbn);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    throw new Exception($"Book with isbn {isbn} not found");
                }

                book = new Book()
                {
                    Isbn = (string)reader["ISBN"],
                    Title = (string)reader["TITLE"],
                    Description = reader["DESCRIPTION"] == DBNull.Value ? null : (string)reader["DESCRIPTION"],
                    Release = (DateTime)reader["RELEASE"],
                    AuthorId = (int)reader["AUTHOR_ID"],
                    Author = new Author()
                    {
                        Id = (int)reader["AUTHOR_ID"],
                        Firstname = (string)reader["FIRST_NAME"],
                        Lastname = (string)reader["LAST_NAME"],
                    },
                };

                connection.Close();
            }

            return View(book.ToBookDetailDto());
        }

        public IActionResult Add()
        {
            List<Author> authors = [];

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"SELECT * FROM AUTHOR";

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    authors.Add(new Author()
                    {
                        Id = (int)reader["ID"],
                        Firstname = (string)reader["FIRST_NAME"],
                        Lastname = (string)reader["LAST_NAME"],
                    });
                }

                connection.Close();
            }

            List<AuthorDto> authorDtos = authors
                .Select(a => a.ToAuthorDto())
                .ToList();

            ViewData.Add("Authors",authorDtos);

            return View(new BookFormDto());
        }

        [HttpPost]
        public IActionResult Add([FromForm] BookFormDto book)
        {

            if (!ModelState.IsValid)
            {
                List<Author> authors = [];

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = connection.CreateCommand();

                    cmd.CommandText = @"SELECT * FROM AUTHOR";

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        authors.Add(new Author()
                        {
                            Id = (int)reader["ID"],
                            Firstname = (string)reader["FIRST_NAME"],
                            Lastname = (string)reader["LAST_NAME"],
                        });
                    }

                    connection.Close();
                }

                List<AuthorDto> authorDtos = authors
                    .Select(a => a.ToAuthorDto())
                    .ToList();

                ViewData.Add("Authors", authorDtos);

                return View(book);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"SELECT cast(CASE 
                                        WHEN EXISTS (SELECT 1 FROM BOOK WHERE isbn = @isbn) 
                                        THEN 1 
                                        ELSE 0 
                                    END as bit) AS isExisting;";

                cmd.Parameters.AddWithValue("@isbn", book.Isbn);

                connection.Open();

                bool exist = (bool)cmd.ExecuteScalar();

                connection.Close();

                if (exist)
                {
                    throw new Exception($"Book with isbn {book.Isbn} already exist");
                }
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"SELECT cast(CASE 
                                        WHEN EXISTS (SELECT 1 FROM AUTHOR WHERE id = @id) 
                                        THEN 1 
                                        ELSE 0 
                                    END as bit) AS isExisting;";

                cmd.Parameters.AddWithValue("@id", book.AuthorId);

                connection.Open();

                bool exist = (bool) cmd.ExecuteScalar();

                connection.Close();

                if(!exist)
                {
                    throw new Exception($"Author with id {book.AuthorId} doesn't exist");
                }
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"INSERT INTO BOOK(ISBN, TITLE, DESCRIPTION, RELEASE, AUTHOR_ID)
                                    VALUES (@isbn, @title, @description, @release, @authorId)";

                cmd.Parameters.AddWithValue("@isbn", book.Isbn);
                cmd.Parameters.AddWithValue("@title", book.Title);
                cmd.Parameters.AddWithValue("@description", book.Description);
                cmd.Parameters.AddWithValue("@release", book.Release);
                cmd.Parameters.AddWithValue("@authorId", book.AuthorId);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();
            }

            return RedirectToAction("Index", "Book");
        }

        [HttpGet("/book/edit/{isbn}")]
        public IActionResult Edit([FromRoute] string isbn)
        {
            Book? book = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @$"SELECT * 
                                    FROM BOOK B 
                                    WHERE ISBN = @isbn";

                cmd.Parameters.AddWithValue("@isbn", isbn);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    throw new Exception($"Book with isbn {isbn} not found");
                }

                book = new Book()
                {
                    Isbn = (string)reader["ISBN"],
                    Title = (string)reader["TITLE"],
                    Description = reader["DESCRIPTION"] == DBNull.Value ? null : (string)reader["DESCRIPTION"],
                    Release = (DateTime)reader["RELEASE"],
                    AuthorId = (int)reader["AUTHOR_ID"]
                };

                connection.Close();
            }

            if (book == null)
            {
                throw new Exception($"Book with isbn {isbn} not found");
            }

            List<Author> authors = [];

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"SELECT * FROM AUTHOR";

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    authors.Add(new Author()
                    {
                        Id = (int)reader["ID"],
                        Firstname = (string)reader["FIRST_NAME"],
                        Lastname = (string)reader["LAST_NAME"],
                    });
                }

                connection.Close();
            }

            List<AuthorDto> authorDtos = authors
                .Select(a => a.ToAuthorDto())
                .ToList();

            ViewData.Add("Authors", authorDtos);

            return View(book.ToBookFormDto());
        }

        [HttpPost("/book/edit/{isbn}")]
        public IActionResult Edit([FromRoute] string isbn, [FromForm] BookFormDto book)
        {
            if (!ModelState.IsValid)
            {
                List<Author> authors = [];

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = connection.CreateCommand();

                    cmd.CommandText = @"SELECT * FROM AUTHOR";

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        authors.Add(new Author()
                        {
                            Id = (int)reader["ID"],
                            Firstname = (string)reader["FIRST_NAME"],
                            Lastname = (string)reader["LAST_NAME"],
                        });
                    }

                    connection.Close();
                }

                List<AuthorDto> authorDtos = authors
                    .Select(a => a.ToAuthorDto())
                    .ToList();

                ViewData.Add("Authors", authorDtos);

                return View(book);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"SELECT cast(CASE 
                                        WHEN EXISTS (SELECT 1 FROM BOOK WHERE isbn = @isbn) 
                                        THEN 1 
                                        ELSE 0 
                                    END as bit) AS isExisting;";

                cmd.Parameters.AddWithValue("@isbn", isbn);

                connection.Open();

                bool exist = (bool)cmd.ExecuteScalar();

                connection.Close();

                if (!exist)
                {
                    throw new Exception($"Book with isbn {book.Isbn} doesn't exist");
                }
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"SELECT cast(CASE 
                                        WHEN EXISTS (SELECT 1 FROM AUTHOR WHERE id = @id) 
                                        THEN 1 
                                        ELSE 0 
                                    END as bit) AS isExisting;";

                cmd.Parameters.AddWithValue("@id", book.AuthorId);

                connection.Open();

                bool exist = (bool)cmd.ExecuteScalar();

                connection.Close();

                if (!exist)
                {
                    throw new Exception($"Author with id {book.AuthorId} doesn't exist");
                }
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"SELECT cast(CASE 
                                        WHEN EXISTS (SELECT 1 FROM BOOK WHERE isbn = @isbn) 
                                        THEN 1 
                                        ELSE 0 
                                    END as bit) AS isExisting;";

                cmd.Parameters.AddWithValue("@isbn", book.Isbn);

                connection.Open();

                bool exist = (bool)cmd.ExecuteScalar();

                connection.Close();

                if (exist)
                {
                    throw new Exception($"Book with isbn {book.Isbn} already exist");
                }
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"UPDATE BOOK 
                                    SET ISBN = @isbn, 
                                        TITLE = @title,
                                        DESCRIPTION = @description, 
                                        RELEASE = @release, 
                                        AUTHOR_ID =@authorId 
                                        WHERE ISBN = @previousIsbn";

                cmd.Parameters.AddWithValue("@isbn",book.Isbn);
                cmd.Parameters.AddWithValue("@title",book.Title);
                cmd.Parameters.AddWithValue("@description",book.Description);
                cmd.Parameters.AddWithValue("@release",book.Release);
                cmd.Parameters.AddWithValue("@authorId",book.AuthorId);
                cmd.Parameters.AddWithValue("@previousIsbn",isbn);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();
            }

            return RedirectToAction("Index", "Book");
        }

        [HttpPost("/book/remove/{isbn}")]
        public IActionResult Remove([FromRoute] string isbn)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"SELECT cast(CASE 
                                        WHEN EXISTS (SELECT 1 FROM BOOK WHERE isbn = @isbn) 
                                        THEN 1 
                                        ELSE 0 
                                    END as bit) AS isExisting;";

                cmd.Parameters.AddWithValue("@isbn", isbn);

                connection.Open();

                bool exist = (bool)cmd.ExecuteScalar();

                connection.Close();

                if (!exist)
                {
                    throw new Exception($"Book with isbn {isbn} doesn't exist");
                }
            }

            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"DELETE FROM BOOK 
                                    WHERE ISBN = @isbn ";

                cmd.Parameters.AddWithValue("@isbn", isbn);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();
            }

            return RedirectToAction("Index", "Book");
        }
    }
}
