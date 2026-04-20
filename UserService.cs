using Microsoft.Data.Sqlite;
using BCrypt.Net;

public class UserService
{
    private string connectionString = "Data Source=medapp.db";

    public UserService()
    {
        CreateTable();
    }

    private void CreateTable()
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Login TEXT UNIQUE,
                PasswordHash TEXT,
                Email TEXT,
                IsDoctor INTEGER,
                IsFirstLogin INTEGER
            );";

            cmd.ExecuteNonQuery();
        }
    }

    // ➕ Створення лікаря
    public void AddDoctor(string login, string password, string email)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(password);

        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO Users (Login, PasswordHash, Email, IsDoctor, IsFirstLogin)
            VALUES (@login, @pass, @email, 1, 1);";

            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@pass", hash);
            cmd.Parameters.AddWithValue("@email", email);

            cmd.ExecuteNonQuery();
        }
    }

    // 🔐 Логін
    public User Login(string login, string password)
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Login = @login";
            cmd.Parameters.AddWithValue("@login", login);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string hash = reader["PasswordHash"].ToString();

                    if (BCrypt.Net.BCrypt.Verify(password, hash))
                    {
                        return new User
                        {
                            Login = reader["Login"].ToString(),
                            Email = reader["Email"].ToString(),
                            IsDoctor = Convert.ToInt32(reader["IsDoctor"]) == 1,
                            IsFirstLogin = Convert.ToInt32(reader["IsFirstLogin"]) == 1
                        };
                    }
                }
            }
        }

        return null;
    }

    // 🔄 Зміна пароля
    public void UpdatePassword(string login, string newPassword)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(newPassword);

        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
            UPDATE Users 
            SET PasswordHash = @pass, IsFirstLogin = 0 
            WHERE Login = @login";

            cmd.Parameters.AddWithValue("@pass", hash);
            cmd.Parameters.AddWithValue("@login", login);

            cmd.ExecuteNonQuery();
        }
    }
}