using System;

public class User
{
    public Guid Id { get; private set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // Admin / Doctor / User
    public bool IsFirstLogin { get; set; }
    public bool IsTemporaryPassword { get; set; }
    public DateTime CreatedAt { get; set; }

    public User()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public User(string login, string password, string role)
    {
        Id = Guid.NewGuid();
        Login = login;
        Password = password;
        Role = role;
        IsFirstLogin = false;
        IsTemporaryPassword = false;
        CreatedAt = DateTime.Now;
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
        IsTemporaryPassword = false;
    }

    // 🔹 Запис у файл
    public string ToFileString()
    {
        return $"{Id};{Login};{Password};{Role};{CreatedAt};{IsTemporaryPassword}";
    }

    // 🔹 Читання з файлу
    public static User FromFileString(string line)
{
    var parts = line.Split(';');

    // 🔥 захист від кривих рядків
    if (parts.Length < 6)
        return null;

    Guid id;
    if (!Guid.TryParse(parts[0], out id))
    {
        id = Guid.NewGuid(); // якщо старий формат
    }

    DateTime createdAt;
    if (!DateTime.TryParse(parts[4], out createdAt))
    {
        createdAt = DateTime.Now;
    }

    bool isTemp = false;
    if (parts.Length > 5)
        bool.TryParse(parts[5], out isTemp);

    return new User
    {
        Id = id,
        Login = parts[1],
        Password = parts[2],
        Role = parts[3],
        CreatedAt = createdAt,
        IsTemporaryPassword = isTemp
    };
}

    public override string ToString()
    {
        return $"[{Role}] {Login} | ID: {Id} | Створено: {CreatedAt}";
    }
}