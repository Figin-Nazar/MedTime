
using System;
using System.Collections.Generic;
using System.IO;

class UserService
{
    private string path = "Users.txt";

    public List<User> LoadUsers()
    {
        List<User> user = new List<User>();

        if (!File.Exists(path))
            return user;

        var lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            var parts = line.Split(';');
            user.Add(new User(parts[0], parts[1], parts[2]));
        }

        return user;
    }

    public void SaveUser(User user)
    {
        string line = $"{user.Login};{user.Password};{user.Role}";
        File.AppendAllText(path, line + "\n");
    }
}
