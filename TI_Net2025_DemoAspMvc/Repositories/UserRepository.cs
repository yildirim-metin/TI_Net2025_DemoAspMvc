using Microsoft.Data.SqlClient;
using TI_Net2025_DemoAspMvc.Models;
using TI_Net2025_DemoAspMvc.Models.Entities;

namespace TI_Net2025_DemoAspMvc.Repositories;

public class UserRepository : BaseRepository<User, int>
{
    protected override string TableName => "USER_";

    protected override string ColumnIdName => "Id";

    public void Add(User user)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = $"""
                INSERT INTO {TableName} (EMAIL, USERNAME, PASSWORD, ROLE)
                VALUES (@email, @username, @password, @role)
                """;
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@role", user.Role.ToString());

            OpenConnection(connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public bool ExistByEmail(string email)
    {
        using (SqlConnection connection = new(_connectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = $"""
                SELECT CAST(
                    CASE WHEN EXISTS ( SELECT 1 FROM {TableName} WHERE EMAIL = @email)
                        THEN 1
                        ELSE 0
                    END as bit) AS isExisting;
                """;

            command.Parameters.AddWithValue("@email", email);

            OpenConnection(connection);

            return (bool)command.ExecuteScalar();
        }
    }

    public bool ExistByUsername(string username)
    {
        using (SqlConnection connection = new(_connectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = $"""
                SELECT CAST(
                    CASE WHEN EXISTS ( SELECT 1 FROM {TableName} WHERE USERNAME = @username)
                        THEN 1
                        ELSE 0
                    END as bit) AS isExisting;
                """;

            command.Parameters.AddWithValue("@username", username);

            OpenConnection(connection);

            return (bool)command.ExecuteScalar();
        }
    }

    public User? GetUserByUsernameOrEmail(string login)
    {
        using (SqlConnection connection = new(_connectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = $"""
                SELECT * FROM {TableName}
                WHERE USERNAME LIKE @login OR EMAIL LIKE @login
                """;

            command.Parameters.AddWithValue("@login", login);

            OpenConnection(connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                return reader.Read() ? MapEntity(reader) : null;
            }
        }
    }

    public override User MapEntity(SqlDataReader reader)
    {
        return new User()
        {
            Id = (int)reader["ID"],
            Email = (string)reader["EMAIL"],
            Username = (string)reader["USERNAME"],
            Password = (string)reader["PASSWORD"],
            Role = Enum.Parse<UserRole>((string)reader["ROLE"]),
        };
    }
}
