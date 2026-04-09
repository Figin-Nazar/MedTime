using System;
using System.Collections.Generic;
using System.IO;

class PatientService
{
    private string folder = "Patients";

    public PatientService()
    {
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
    }

    private string GetPath(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new Exception("Логін пустий!");

        login = login.Trim().ToLower();

        foreach (char c in Path.GetInvalidFileNameChars())
        {
            login = login.Replace(c.ToString(), "");
        }

        return Path.Combine(folder, login + ".txt");
    }

    public void CreatePatient(string login, string allergies, string chronic, string other)
    {
        string path = GetPath(login);

        if (!File.Exists(path))
        {
            File.WriteAllText(path,
$@"# PROFILE
Allergies: {allergies}
Chronic: {chronic}
Other: {other}

# CURRENT

# HISTORY
");
        }
    }

    public void CreatePatient(string login)
    {
        CreatePatient(login, "немає", "немає", "немає");
    }

    public void ShowPatient(string login)
    {
        string path = GetPath(login);

        if (!File.Exists(path))
        {
            Console.WriteLine("Профіль не знайдено");
            return;
        }

        Console.WriteLine(File.ReadAllText(path));
    }

    public void AddMedicine(string login, string med)
    {
        string path = GetPath(login);

        if (!File.Exists(path))
        {
            Console.WriteLine("Профіль не знайдено");
            return;
        }

        var lines = File.ReadAllLines(path);
        List<string> newLines = new List<string>();

        bool added = false;

        foreach (var line in lines)
        {
            newLines.Add(line);

            if (line.Trim() == "# CURRENT" && !added)
            {
                newLines.Add(med);
                added = true;
            }
        }

        File.WriteAllLines(path, newLines);
    }

    public void ShowMedicines(string login)
    {
        string path = GetPath(login);

        if (!File.Exists(path))
        {
            Console.WriteLine("Немає даних");
            return;
        }

        var lines = File.ReadAllLines(path);

        bool show = false;

        foreach (var line in lines)
        {
            if (line.Trim() == "# CURRENT")
            {
                show = true;
                continue;
            }

            if (line.Trim() == "# HISTORY")
            {
                show = false;
            }

            if (show && !string.IsNullOrWhiteSpace(line))
            {
                Console.WriteLine(line);
            }
        }
    }
}