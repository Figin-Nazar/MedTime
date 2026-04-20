using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

   
    public void CreatePatient(string login, string allergies = "немає", string chronic = "немає", string other = "немає")
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

        var lines = File.ReadAllLines(path).ToList();

        int currentCount = lines.Count(l => l.Contains(";"));
        if (currentCount >= 20)
        {
            Console.WriteLine("Досягнуто максимум ліків (20)");
            return;
        }

        var meds = lines
            .Where(l => l.Contains(";"))
            .Select(l => l.Split(';'))
            .Where(p => p.Length == 3)
            .OrderBy(p => TimeSpan.Parse(p[2]))
            .ToList();

        var newMed = med.Split(';');

        if (newMed.Length != 3)
        {
            Console.WriteLine("Невірний формат ліків");
            return;
        }

        meds.Add(newMed);

        meds = meds.OrderBy(p => TimeSpan.Parse(p[2])).ToList();

        List<string> newLines = new List<string>();
        bool inCurrent = false;

        foreach (var line in lines)
        {
            if (line.Trim() == "# CURRENT")
            {
                newLines.Add(line);
                inCurrent = true;

                foreach (var m in meds)
                    newLines.Add($"{m[0]};{m[1]};{m[2]}");

                continue;
            }

            if (line.Trim() == "# HISTORY")
                inCurrent = false;

            if (!inCurrent)
                newLines.Add(line);
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
        bool empty = true;

        foreach (var line in lines)
        {
            if (line.Trim() == "# CURRENT")
            {
                show = true;
                continue;
            }

            if (line.Trim() == "# HISTORY")
                show = false;

            if (show && !string.IsNullOrWhiteSpace(line))
            {
                var parts = line.Split(';');

                if (parts.Length == 3)
                {
                    Console.WriteLine($"💊 {parts[0]} | Доза: {parts[1]} | Час: {parts[2]}");
                }

                empty = false;
            }
        }

        if (empty)
            Console.WriteLine("Список ліків пустий");
    }
}