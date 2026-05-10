using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Console1
{
    class MedicineCourse
    {
        private string GetPath(string login)
        {
            string folder = "Patients";

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            login = login.Trim().ToLower();

            foreach (char c in Path.GetInvalidFileNameChars())
                login = login.Replace(c.ToString(), "");

            return Path.Combine(folder, login + ".txt");
        }

        // ================= ДОДАВАННЯ ЛІКІВ =================
        public void AddMedicine(string login)
        {
            string path = GetPath(login);

            Console.Write("Назва: ");
            string name = Console.ReadLine();

            Console.Write("Доза: ");
            string dose = Console.ReadLine();

            Console.Write("Час (hh:mm): ");
            string time = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(dose) ||
                string.IsNullOrWhiteSpace(time))
            {
                Console.WriteLine("Всі поля обов'язкові!");
                return;
            }

            if (!TimeSpan.TryParse(time, out _))
            {
                Console.WriteLine("Невірний формат часу!");
                return;
            }

            // 🔥 ВАЖЛИВО: дата автоматично + статус WAIT
            File.AppendAllText(path,
                $"{DateTime.Today:dd.MM.yyyy};{name};{dose};{time};WAIT\n");

            Console.WriteLine("Додано ✅");
        }

        // ================= ПОКАЗ ЛІКІВ =================
        public void ShowMedicines(string login)
        {
            string path = GetPath(login);

            if (!File.Exists(path))
            {
                Console.WriteLine("Немає даних");
                return;
            }

            var lines = File.ReadAllLines(path);
            int index = 1;

            foreach (var l in lines)
            {
                var p = l.Split(';');

                if (p.Length >= 5)
                {
                    string status = p[4] switch
                    {
                        "1" => "✔",
                        "-1" => "❌",
                        "WAIT" => "⏳",
                        _ => "⏳"
                    };

                    Console.WriteLine($"{index}. 📅 {p[0]} | 💊 {p[1]} | {p[2]} | {p[3]} | {status}");
                    index++;
                }
            }
        }

        // ================= ВІДМІТКА =================
        public void MarkMedicine(string login)
        {
            string path = GetPath(login);

            if (!File.Exists(path))
            {
                Console.WriteLine("Немає даних");
                return;
            }

            var lines = File.ReadAllLines(path).ToList();

            var meds = lines
                .Where(l => l.Split(';').Length >= 5)
                .ToList();

            if (meds.Count == 0)
            {
                Console.WriteLine("Немає ліків");
                return;
            }

            for (int i = 0; i < meds.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {meds[i]}");
            }

            Console.Write("Обери номер: ");
            if (!int.TryParse(Console.ReadLine(), out int index) ||
                index < 1 || index > meds.Count)
            {
                Console.WriteLine("Невірний номер");
                return;
            }

            var selected = meds[index - 1];
            var parts = selected.Split(';');

            // 🔥 НЕ МОЖНА змінити прострочене
            if (parts[4] == "❌")
            {
                Console.WriteLine("⛔ Прострочено!");
                return;
            }

            Console.WriteLine("1 - Випив ✔");
            Console.WriteLine("2 - Не випив ❌");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Помилка вводу");
                return;
            }

            parts[4] = choice == 1 ? "1" : "-1";

            lines[lines.IndexOf(selected)] = string.Join(";", parts);

            File.WriteAllLines(path, lines);

            Console.WriteLine("Оновлено ✅");
        }

        // ================= АВТО-ПЕРЕВІРКА =================
        public void CheckExpired(string login)
        {
            string path = GetPath(login);

            if (!File.Exists(path))
                return;

            var lines = File.ReadAllLines(path).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                var p = lines[i].Split(';');

                if (p.Length < 5)
                    continue;

                if (p[4] == "1" || p[4] == "-1")
                    continue;

                if (!DateTime.TryParse($"{p[0]} {p[3]}", out DateTime medTime))
                    continue;

                if (DateTime.Now > medTime.AddMinutes(10))
                {
                    p[4] = "-1";
                    lines[i] = string.Join(";", p);
                }
            }

            File.WriteAllLines(path, lines);
        }
    }
}