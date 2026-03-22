using System;

public class User
{
    // 🔹 Дані для входу
    public string Login { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // Admin / Doctor / User
    public bool IsFirstLogin { get; set; }

    // 🔹 Системні поля
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    // 🔹 Пустий конструктор (для читання з файлу)
    public User() { }

    // 🔹 Основний конструктор
    public User(string login, string password, string role)
    {
        Id = Guid.NewGuid();
        Login = login;
        Password = password;
        Role = role;
        CreatedAt = DateTime.Now;
        IsFirstLogin = false;
    }

    // 🔹 Перевірка пароля
    public bool CheckPassword(string password)
    {
        return Password == password;
    }

    // 🔹 Зміна пароля
    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("Пароль не може бути пустим");

        Password = newPassword;
        IsFirstLogin = false;
    }

    // 🔹 Запис у файл
    public string ToFileString()
    {
        return $"{Id};{Login};{Password};{Role};{CreatedAt};{IsFirstLogin}";
    }

    // 🔹 Читання з файлу (ВАЖЛИВО: з перевіркою)
    public static User FromFileString(string line)
    {
        try
        {
            var parts = line.Split(';');

            if (parts.Length < 6)
                throw new Exception("Невірний формат рядка");

            return new User
            {
                Id = Guid.Parse(parts[0]),
                Login = parts[1],
                Password = parts[2],
                Role = parts[3],
                CreatedAt = DateTime.Parse(parts[4]),
                IsFirstLogin = bool.Parse(parts[5])
            };
        }
        catch
        {
            Console.WriteLine("❌ Помилка читання рядка: " + line);
            return null;
        }
    }

    // 🔹 Вивід
    public override string ToString()
    {
        return $"[{Role}] {Login} | ID: {Id} | Створено: {CreatedAt}";
    }
}