using System;
using Microsoft.Data.Sqlite;

namespace Console1
{
    public class DoctorService
    {
        private readonly DatabaseService _db = new DatabaseService();

        public User Login(string login, string password)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Doctors WHERE Login=$login AND Password=$pass";

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

        public void UpdatePassword(string login, string newPass)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
UPDATE Doctors 
SET Password=$pass, IsTemporaryPassword=0
WHERE Login=$login";

            cmd.Parameters.AddWithValue("$pass", newPass);
            cmd.Parameters.AddWithValue("$login", login);

            cmd.ExecuteNonQuery();
        }
    }
}