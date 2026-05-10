using System;
using System.Linq;

public class AuthService
{
    private static Random random = new Random();

    // 🔐 Генерація пароля
    public string GeneratePassword(int length = 8)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    // Генерація логіну
    public string GenerateLogin(string firstName, string lastName)
    {
        string baseLogin = (firstName + "." + lastName).ToLower();

        // додаємо випадкове число щоб уникнути дублювання
        int num = random.Next(100, 999);

        return $"{baseLogin}{num}";
    }
}