using System;

public class User
{
    
    public string Login { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // Admin / Doctor / User
    public bool IsFirstLogin { get; set; }

    
    public Guid Id { get; private set; } = Guid.NewGuid(); //  авто-унікальний
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    
    public User() { }

    
    public User(string login, string password, string role)
    {
        Login = login;
        Password = password;
        Role = role;
        IsFirstLogin = false;
    }

    //  Перевірка пароля
    public bool CheckPassword(string password)
    {
        return Password == password;
    }

    //  Зміна пароля
    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("Пароль не може бути пустим");

        Password = newPassword;
        IsFirstLogin = false;
    }

    //  Запис у файл
    public string ToFileString()
    {
        return $"{Id};{Login};{Password};{Role};{CreatedAt};{IsFirstLogin}";
    }

    //  Читання з файлу
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

    //  Вивід
    public override string ToString()
    {
        return $"[{Role}] {Login} | ID: {Id} | Створено: {CreatedAt}";
    }
}