using Microsoft.Data.SqlClient;
using TI_Net2025_DemoAspMvc.Models.Entities;

namespace TI_Net2025_DemoAspMvc.Repositories
{
    public class BookRepository
    {
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=BookDb;Trusted_Connection=True;";

        public List<Book> GetAllBooksWithAuthor()
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

            return books;
        }

        public Book? GetOneWithAuthor(string isbn)
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
                    return null;
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

            return book;
        }

        public Book? GetOne(string isbn)
        {
            Book book;

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
                    return null;
                }

                book = new Book()
                {
                    Isbn = (string)reader["ISBN"],
                    Title = (string)reader["TITLE"],
                    Description = reader["DESCRIPTION"] == DBNull.Value ? null : (string)reader["DESCRIPTION"],
                    Release = (DateTime)reader["RELEASE"],
                    AuthorId = (int)reader["AUTHOR_ID"],
                };

                connection.Close();
            }

            return book;
        }

        public bool ExistByIsbn(string isbn)
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

                return exist;
            }
        }

        public void Insert(Book book)
        {
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
        }

        public void Update(string isbn, Book book)
        {
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

                cmd.Parameters.AddWithValue("@isbn", book.Isbn);
                cmd.Parameters.AddWithValue("@title", book.Title);
                cmd.Parameters.AddWithValue("@description", book.Description);
                cmd.Parameters.AddWithValue("@release", book.Release);
                cmd.Parameters.AddWithValue("@authorId", book.AuthorId);
                cmd.Parameters.AddWithValue("@previousIsbn", isbn);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(string isbn)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"DELETE FROM BOOK 
                                    WHERE ISBN = @isbn ";

                cmd.Parameters.AddWithValue("@isbn", isbn);

                connection.Open();

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
