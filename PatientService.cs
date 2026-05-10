using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class PatientService
{
    private string folder = "Patients";
    public void CheckExpiredMedicines(string login)
    {
        string path = PathHelper.GetPath(login);

        if (!File.Exists(path))
            return;

        var lines = File.ReadAllLines(path).ToList();
        bool updated = false;

        for (int i = 0; i < lines.Count; i++)
        {
            var parts = lines[i].Split(';');

            if (parts.Length < 5)
                continue;

            string date = parts[0];
            string time = parts[3];
            string status = parts[4];

            // якщо вже відмічено — пропускаємо
            if (status == "✔" || status == "❌")
                continue;

            // парсимо дату+час
            if (!DateTime.TryParse($"{date} {time}", out DateTime medTime))
                continue;

            // якщо пройшло більше 10 хв
            if (DateTime.Now > medTime.AddMinutes(10))
            {
                parts[4] = "❌";
                lines[i] = string.Join(";", parts);
                updated = true;
            }
        }

        if (updated)
            File.WriteAllLines(path, lines);
    }





    public void MarkMedicine(string login)
    {
        string path = PathHelper.GetPath(login);

        if (!File.Exists(path))
        {
            Console.WriteLine("Немає даних");
            return;
        }

        var lines = File.ReadAllLines(path).ToList();

        var meds = lines
            .Where(l => l.Contains(";"))
            .Select((line, index) => new { line, index })
            .ToList();

        if (meds.Count == 0)
        {
            Console.WriteLine("Немає ліків");
            return;
        }

        Console.WriteLine("\nСписок ліків:");

        for (int i = 0; i < meds.Count; i++)
        {
            var parts = meds[i].line.Split(';');

            if (parts.Length >= 3)
            {
                string status = parts.Length == 4 ? parts[3] : "❓";

                Console.WriteLine($"{i + 1}. {parts[0]} | {parts[1]} | {parts[2]} | {status}");
            }
        }

        Console.Write("Обери номер: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > meds.Count)
        {
            Console.WriteLine("Невірний вибір");
            return;
        }

        Console.Write("1 - Випив ✔, 2 - Не випив ❌: ");
        string result = Console.ReadLine();

        string mark = result == "1" ? "✔" : "❌";

        var selected = meds[choice - 1];
        var partsSelected = selected.line.Split(';').ToList();

        if (partsSelected.Count >= 4)
            partsSelected[3] = mark;
        else
            partsSelected.Add(mark);

        lines[selected.index] = string.Join(";", partsSelected);

        File.WriteAllLines(path, lines);

        Console.WriteLine("Збережено!");
    }

    public PatientService()
    {
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
    }

    private string GetPath(string login)
    {
        login = login.Trim().ToLower();
        return Path.Combine(folder, login + ".txt");
    }

    public void CreatePatient(string login)
    {
        string path = GetPath(login);

        if (!File.Exists(path))
        {
            File.WriteAllText(path,
@"# PROFILE

# MEDICINES
");
        }
    }

    public void AddMedicine(string login)
    {
        string path = GetPath(login);

        Console.Write("Дата (dd.MM.yyyy): ");
        string date = Console.ReadLine();

        Console.Write("Назва: ");
        string name = Console.ReadLine();

        Console.Write("Доза: ");
        string dose = Console.ReadLine();

        Console.Write("Час: ");
        string time = Console.ReadLine();

        File.AppendAllText(path, $"{date};{name};{dose};{time}\n");
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

        foreach (var l in lines)
        {
            var p = l.Split(';');
            if (p.Length == 4)
                Console.WriteLine($"📅 {p[0]} | 💊 {p[1]} | {p[2]} | {p[3]}");
        }
    }

    public void RemoveMedicine(string login)
    {
        string path = GetPath(login);

        var lines = File.ReadAllLines(path).ToList();
        var meds = lines.Where(l => l.Contains(";")).ToList();

        for (int i = 0; i < meds.Count; i++)
            Console.WriteLine($"{i + 1}. {meds[i]}");

        Console.Write("Видалити №: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        lines.Remove(meds[index]);

        File.WriteAllLines(path, lines);
    }
}