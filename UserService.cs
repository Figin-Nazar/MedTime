using System;
using System.Collections.Generic;
using System.IO;

public class UserService
{
    private string path = "users.txt";

    public UserService()
    {
        if (!File.Exists(path))
            File.Create(path).Close();
    }

    //  ЗБЕРЕГТИ користувача
    public void SaveUser(User user)
    {
        user.Login = user.Login.Trim(); 
        user.Password = user.Password.Trim();

        if (string.IsNullOrWhiteSpace(user.Login))
            throw new Exception("Пустий логін");

        File.AppendAllText(path, user.ToFileString() + Environment.NewLine);
    }

    // ЗАВАНТАЖИТИ всіх
    public List<User> LoadUsers()
    {
        var users = new List<User>();

        var lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            var user = User.FromFileString(line);
            if (user != null)
                users.Add(user);
        }

        return users;
    }
}