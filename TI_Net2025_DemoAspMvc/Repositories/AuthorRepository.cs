using Microsoft.Data.SqlClient;
using TI_Net2025_DemoAspMvc.Models.Entities;

namespace TI_Net2025_DemoAspMvc.Repositories
{
    public class AuthorRepository
    {

        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=BookDb;Trusted_Connection=True;";

        public List<Author> GetAll()
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

            return authors;
        }

        public bool ExistById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = @"SELECT cast(CASE 
                                        WHEN EXISTS (SELECT 1 FROM AUTHOR WHERE id = @id) 
                                        THEN 1 
                                        ELSE 0 
                                    END as bit) AS isExisting;";

                cmd.Parameters.AddWithValue("@id", id);

                connection.Open();

                bool exist = (bool)cmd.ExecuteScalar();

                connection.Close();

                return exist;
            }
        }
    }
}
