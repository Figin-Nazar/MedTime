using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    private static void Main()
    {
        UserService userService = new UserService();
        DoctorService doctorService = new DoctorService();
        PatientService patientService = new PatientService();

        List<User> users = userService.LoadUsers();
        List<User> doctors = doctorService.LoadDoctors();

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        bool run = true;

        while (run)
        {
            Console.WriteLine("\n1 - Лікар");
            Console.WriteLine("2 - Користувач");
            Console.WriteLine("3 - Реєстрація");
            Console.WriteLine("0 - Вихід");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                continue;

            // ЛІКАР
            if (choice == 1)
            {
                Console.Write("Логін: ");
                string login = Console.ReadLine()?.Trim().ToLower();

                Console.Write("Пароль: ");
                string password = Console.ReadLine();

                User doctor = doctors.Find(d =>
                    d.Login.ToLower() == login && d.Password == password);

                if (doctor == null)
                {
                    Console.WriteLine("Невірні дані");
                    continue;
                }

                Console.WriteLine("Вхід як лікар");

                while (true)
                {
                    Console.WriteLine("\n1 - Список пацієнтів");
                    Console.WriteLine("2 - Відкрити пацієнта");
                    Console.WriteLine("0 - Назад");

                    int docChoice = Convert.ToInt32(Console.ReadLine());

                    if (docChoice == 1)
                    {
                        foreach (var u in users)
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

                            int pChoice = Convert.ToInt32(Console.ReadLine());

                            if (pChoice == 1)
                                patientService.ShowPatient(pLogin);

                            else if (pChoice == 2)
                            {
                                Console.Write("Назва: ");
                                string name = Console.ReadLine();

                                Console.Write("Доза: ");
                                string dose = Console.ReadLine();

                                Console.Write("Час: ");
                                string time = Console.ReadLine();

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
                string password = Console.ReadLine();

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

                    int uChoice = Convert.ToInt32(Console.ReadLine());

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

                Console.Write("Пароль: ");
                string password = Console.ReadLine();

                if (users.Exists(u => u.Login.ToLower() == login))
                {
                    Console.WriteLine("Такий логін вже є");
                    continue;
                }

                Console.Write("Алергії: ");
                string allergies = Console.ReadLine();

                Console.Write("Хронічні: ");
                string chronic = Console.ReadLine();

                Console.Write("Інші: ");
                string other = Console.ReadLine();

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
    }
}