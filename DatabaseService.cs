using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Console1
{
    public class DatabaseService
    {
        private readonly string connectionString = "Data Source=myapp.db";

        public DatabaseService()
        {
            Init();
        }

        private void Init()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            CreateTables(connection);
            ApplyMigrations(connection);
        }

        // 🧱 СТВОРЕННЯ ТАБЛИЦЬ
        private void CreateTables(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();

            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Users (
    Id TEXT PRIMARY KEY,
    Login TEXT UNIQUE,
    Password TEXT,
    Email TEXT
);

CREATE TABLE IF NOT EXISTS Doctors (
    Id TEXT PRIMARY KEY,
    Login TEXT UNIQUE,
    Password TEXT,
    Email TEXT
);
";

            cmd.ExecuteNonQuery();
        }

        // 🚀 МІГРАЦІЇ (ГОЛОВНА ФІШКА)
        private void ApplyMigrations(SqliteConnection connection)
        {
            EnsureColumn(connection, "Users", "IsTemporaryPassword", "INTEGER DEFAULT 0");
            EnsureColumn(connection, "Doctors", "IsTemporaryPassword", "INTEGER DEFAULT 0");
        }

        // 🧠 ПЕРЕВІРКА І ДОДАВАННЯ КОЛОНКИ
        private void EnsureColumn(SqliteConnection connection, string table, string column, string type)
        {
            var columns = GetColumns(connection, table);

            if (!columns.Contains(column))
            {
                Console.WriteLine($"[МІГРАЦІЯ] Додаю колонку {column} в {table}");

                var cmd = connection.CreateCommand();
                cmd.CommandText = $"ALTER TABLE {table} ADD COLUMN {column} {type}";
                cmd.ExecuteNonQuery();
            }
        }

        // 📋 ОТРИМАННЯ СПИСКУ КОЛОНОК
        private HashSet<string> GetColumns(SqliteConnection connection, string table)
        {
            var columns = new HashSet<string>();

            var cmd = connection.CreateCommand();
            cmd.CommandText = $"PRAGMA table_info({table});";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                columns.Add(reader["name"].ToString());
            }

            return columns;
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
    }
}