using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Console1
{
    public class UserService
    {
        private readonly DatabaseService _db = new DatabaseService();

        
        public void Register(User user)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO Users (Id, Login, Password, Email, IsTemporaryPassword)
VALUES ($id, $login, $pass, $email, $temp)";

            cmd.Parameters.AddWithValue("$id", user.Id.ToString());
            cmd.Parameters.AddWithValue("$login", user.Login);
            cmd.Parameters.AddWithValue("$pass", user.Password);
            cmd.Parameters.AddWithValue("$email", user.Email ?? "");
            cmd.Parameters.AddWithValue("$temp", user.IsTemporaryPassword ? 1 : 0);

            cmd.ExecuteNonQuery();
        }

        public User Login(string login, string password)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Login=$login AND Password=$pass";

            cmd.Parameters.AddWithValue("$login", login);
            cmd.Parameters.AddWithValue("$pass", password);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    Login = reader["Login"].ToString(),
                    Password = reader["Password"].ToString(),
                    Email = reader["Email"].ToString(),
                    IsTemporaryPassword = Convert.ToInt32(reader["IsTemporaryPassword"]) == 1
                };
            }

            return null;
        }
    }
}