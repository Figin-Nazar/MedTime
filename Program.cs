using System;
using System.Collections.Generic;
using System.Text;
using Console1;

class Program
{
    private static void Main()
    {
        UserService userService = new UserService();
        PatientService patientService = new PatientService();
        MedUserService medUserService = new MedUserService();

        List<User> users = userService.LoadUsers();      
        List<User> doctors = medUserService.LoadDoctors();

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        bool run = true;

        while (run)
        {
            try // 
            {
                Console.WriteLine("\n1 - Лікар");
                Console.WriteLine("2 - Користувач");
                Console.WriteLine("3 - Реєстрація");
                Console.WriteLine("0 - Вихід");

                if (!int.TryParse(Console.ReadLine(), out int choice)) 
                {
                    Console.WriteLine("Введіть число!");
                    continue;
                }

                // ЛІКАР
if (choice == 1)
{
    Console.Write("Логін: ");
    string login = Console.ReadLine()?.Trim().ToLower();

    Console.Write("Пароль: ");

    string password = Console.ReadLine()?.Trim();

    User doctor = doctors.Find(d =>
        d.Login.ToLower() == login && d.Password == password);

    if (doctor == null)
    {
        Console.WriteLine("Невірні дані");
        continue;
    }

    if (doctor.IsTemporaryPassword)
    {
        Console.WriteLine("У вас тимчасовий пароль! Потрібно змінити.");

        Console.Write("Новий пароль: ");
        string newPassword = Console.ReadLine();

        doctor.Password = newPassword;
        doctor.IsTemporaryPassword = false;

        
        medUserService.SaveAllDoctors(doctors);

        Console.WriteLine("Пароль змінено!");
    }

    Console.WriteLine("Вхід успішний!");

    //  МЕНЮ ЛІКАРЯ
    while (true)
    {
        Console.WriteLine("\n1 - Список пацієнтів");
        Console.WriteLine("2 - Відкрити пацієнта");
        Console.WriteLine("0 - Назад");

        if (!int.TryParse(Console.ReadLine(), out int docChoice))
        {
            Console.WriteLine("Введіть число!");
            continue;
        }

        if (docChoice == 1)
        {
            if (users.Count == 0)
            {
                Console.WriteLine("Список пустий");
                continue;
            }

            foreach (var u in users.OrderBy(u => u.Login))
                Console.WriteLine(u.Login);
        }

        else if (docChoice == 2)
        {
            Console.Write("Логін пацієнта: ");
            string pLogin = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(pLogin))
            {
                Console.WriteLine("Помилка логіну");
                continue;
            }

            patientService.CreatePatient(pLogin);

            while (true)
            {
                Console.WriteLine("\n1 - Профіль");
                Console.WriteLine("2 - Додати ліки");
                Console.WriteLine("0 - Назад");

                if (!int.TryParse(Console.ReadLine(), out int pChoice))
                {
                    Console.WriteLine("Введіть число!");
                    continue;
                }

                if (pChoice == 1)
                    patientService.ShowPatient(pLogin);

                else if (pChoice == 2)
                {
                    Console.Write("Назва: ");
                    string name = Console.ReadLine()?.Trim();

                    Console.Write("Доза: ");
                    string dose = Console.ReadLine()?.Trim();

                    Console.Write("Час (hh:mm): ");
                    string time = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(name) ||
                        string.IsNullOrWhiteSpace(dose) ||
                        string.IsNullOrWhiteSpace(time))
                    {
                        Console.WriteLine("Всі поля повинні бути заповнені");
                        continue;
                    }

                    if (!TimeSpan.TryParse(time, out _))
                    {
                        Console.WriteLine("Невірний формат часу");
                        continue;
                    }

                    patientService.AddMedicine(pLogin, $"{name};{dose};{time}");
                }

                else if (pChoice == 0)
                    break;
            }
        }

        else if (docChoice == 0)
            break;
    }
}
                    
            

                // КОРИСТУВАЧ
                else if (choice == 2)
                {
                    Console.Write("Логін: ");
                    string login = Console.ReadLine()?.Trim().ToLower();

                    Console.Write("Пароль: ");
                    string password = Console.ReadLine()?.Trim(); 

                    User user = users.Find(u =>
                        u.Login.ToLower() == login && u.Password == password);

                    if (user == null)
                    {
                        Console.WriteLine("Невірні дані");
                        continue;
                    }

                    Console.WriteLine("Вхід як користувач");

                    while (true)
                    {
                        Console.WriteLine("\n1 - Мій графік");
                        Console.WriteLine("0 - Вийти");

                        if (!int.TryParse(Console.ReadLine(), out int uChoice)) 
                        {
                            Console.WriteLine("Введіть число!");
                            continue;
                        }

                        if (uChoice == 1)
                            patientService.ShowMedicines(login);

                        else if (uChoice == 0)
                            break;
                    }
                }

                // РЕЄСТРАЦІЯ
                else if (choice == 3)
                {
                    Console.Write("Логін: ");
                    string login = Console.ReadLine()?.Trim().ToLower();

                    if (string.IsNullOrWhiteSpace(login))
                    {
                        Console.WriteLine("Пустий логін");
                        continue;
                    }

                    if (login.Length > 30) 
                    {
                        Console.WriteLine("Логін занадто довгий");
                        continue;
                    }

                    Console.Write("Пароль: ");
                    string password = Console.ReadLine()?.Trim(); 

                    if (string.IsNullOrWhiteSpace(password)) 
                    {
                        Console.WriteLine("Пустий пароль");
                        continue;
                    }

                    if (users.Exists(u => u.Login.ToLower() == login))
                    {
                        Console.WriteLine("Такий логін вже є");
                        continue;
                    }

                    Console.Write("Алергії: ");
                    string allergies = Console.ReadLine()?.Trim(); 

                    Console.Write("Хронічні: ");
                    string chronic = Console.ReadLine()?.Trim(); 

                    Console.Write("Інші: ");
                    string other = Console.ReadLine()?.Trim(); 

                    User newUser = new User(login, password, "User");
                    userService.SaveUser(newUser);

                    patientService.CreatePatient(login, allergies, chronic, other);

                    users = userService.LoadUsers();

                    Console.WriteLine("Готово");
                }

                else if (choice == 0)
                {
                    run = false;
                }
                    }
            catch (Exception ex) // ✅ NEW: глобальна обробка
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }}
