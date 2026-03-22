using System;
using System.Collections.Generic;
using System.Text;

namespace Console1
{
    class AuthService
    {
        private Random rnd = new Random();

        public string GenerateTempPassword()
        {
            return rnd.Next(100000, 999999).ToString();
        }

        public void SendCredentials(string login, string password)
        {
            Console.WriteLine("=== Відправка даних ===");
            Console.WriteLine($"Login: {login}");
            Console.WriteLine($"Password: {password}");
        }
    }
}