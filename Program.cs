using SQLitePCL;
using System;
using System.Text;

class Program
{
    private static void Main()
    {
        Batteries.Init();

        var userService = new UserService();
        var emailService = new EmailService();
        var doctorService = new DoctorService(userService, emailService);
        var patientService = new PatientService();

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        bool run = true;

        while (run)
        {
            try
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

                // ================= ЛІКАР =================
                if (choice == 1)
                {
                    Console.Write("Логін: ");
                    string login = Console.ReadLine()?.Trim().ToLower();

                    Console.Write("Пароль: ");
                    string password = Console.ReadLine()?.Trim();

                    User doctor = userService.Login(login, password);
                    if (doctor == null || !doctor.IsDoctor)
                    {
                    
                        Console.WriteLine("Невірні дані");
                        continue;
                    }

                    if (doctor.IsFirstLogin)
                    {
                        Console.WriteLine("Перший вхід — змініть пароль");

                        Console.Write("Новий пароль: ");
                        string newPassword = Console.ReadLine();

                        userService.UpdatePassword(login, newPassword);

                        Console.WriteLine("Пароль змінено");
                        continue;
                    }

                    Console.WriteLine("Вхід як лікар");

                    while (true)
                    {
                        Console.WriteLine("\n1 - Список пацієнтів");
                        Console.WriteLine("2 - Відкрити пацієнта");
                        Console.WriteLine("0 - Назад");

                        if (!int.TryParse(Console.ReadLine(), out int docChoice))
                            continue;

                        if (docChoice == 1)
                        {
                            Console.WriteLine("Список поки через SQL не зробили 😄");
                        }
                        else if (docChoice == 2)
                        {
                            Console.Write("Логін пацієнта: ");
                            string pLogin = Console.ReadLine()?.Trim().ToLower();

                            patientService.CreatePatient(pLogin);

                            while (true)
                            {
                                Console.WriteLine("\n1 - Профіль");
                                Console.WriteLine("2 - Додати ліки");
                                Console.WriteLine("0 - Назад");

                                if (!int.TryParse(Console.ReadLine(), out int pChoice))
                                    continue;

                                if (pChoice == 1)
                                    patientService.ShowPatient(pLogin);

                                else if (pChoice == 2)
                                {
                                    Console.Write("Назва: ");
                                    string name = Console.ReadLine();

                                    Console.Write("Доза: ");
                                    string dose = Console.ReadLine();

                                    Console.Write("Час (hh:mm): ");
                                    string time = Console.ReadLine();

                                    if (!TimeSpan.TryParse(time, out _))
                                    {
                                        Console.WriteLine("Невірний час");
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

                // ================= КОРИСТУВАЧ =================
                else if (choice == 2)
                {
                    Console.Write("Логін: ");
                    string login = Console.ReadLine()?.Trim().ToLower();

                    Console.Write("Пароль: ");
                    string password = Console.ReadLine()?.Trim();

                    var user = userService.Login(login, password);

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
                            continue;

                        if (uChoice == 1)
                            patientService.ShowMedicines(login);

                        else if (uChoice == 0)
                            break;
                    }
                }

                // ================= РЕЄСТРАЦІЯ =================
                else if (choice == 3)
                {
                    Console.Write("Логін: ");
                    string login = Console.ReadLine()?.Trim().ToLower();

                    Console.Write("Пароль: ");
                    string password = Console.ReadLine()?.Trim();

                    Console.Write("Email: ");
                    string email = Console.ReadLine()?.Trim();

                    userService.AddDoctor(login, password, email); // або окремо AddUser зробиш

                    Console.WriteLine("Зареєстровано");
                }

                else if (choice == 0)
                {
                    run = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}