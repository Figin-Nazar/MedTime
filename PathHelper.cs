using System;
using System.IO;

public static class PathHelper
{
    private static string folder = "Patients";

    public static string GetPath(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new Exception("Логін пустий!");

        login = login.Trim().ToLower();

        foreach (char c in Path.GetInvalidFileNameChars())
            login = login.Replace(c.ToString(), "");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        return Path.Combine(folder, login + ".txt");
    }
}