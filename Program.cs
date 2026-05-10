using System;
using System.Text;
using Console1;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var userService = new UserService();
        var doctorService = new DoctorService();
        var patientService = new PatientService();

        while (true)
        {
            Console.WriteLine("\n1 - Лікар");
            Console.WriteLine("2 - Користувач");
            Console.WriteLine("3 - Реєстрація");
            Console.WriteLine("0 - Вихід");

            int choice = int.Parse(Console.ReadLine());

            // ЛІКАР
            if (choice == 1)
            {
                Console.Write("Логін: ");
                string login = Console.ReadLine();

                Console.Write("Пароль: ");
                string pass = Console.ReadLine();

                var doctor = doctorService.Login(login, pass);

                if (doctor == null)
                {
                    Console.WriteLine("Невірні дані");
                    continue;
                }

                if (doctor.IsTemporaryPassword)
                {
                    Console.WriteLine("Змініть пароль:");
                    string newPass = Console.ReadLine();
                    doctorService.UpdatePassword(login, newPass);
                }

                Console.Write("Пацієнт login: ");
                string pLogin = Console.ReadLine();

                patientService.CreatePatient(pLogin);

                while (true)
                {
                    Console.WriteLine("\n1 - Додати ліки");
                    Console.WriteLine("2 - Видалити ліки");
                    Console.WriteLine("3 - Показати");
                    Console.WriteLine("0 - Назад");

                    int d = int.Parse(Console.ReadLine());

                    if (d == 1) patientService.AddMedicine(pLogin);
                    else if (d == 2) patientService.RemoveMedicine(pLogin);
                    else if (d == 3) patientService.ShowMedicines(pLogin);
                    else break;
                }
            }

            // КОРИСТУВАЧ
            else if (choice == 2)
            {
                Console.Write("Логін: ");
                string login = Console.ReadLine();

                Console.Write("Пароль: ");
                string pass = Console.ReadLine();

                var user = userService.Login(login, pass);

                while (true)
                {
                    Console.WriteLine("\n1 - Показати ліки");
                    Console.WriteLine("2 - Відмітити прийом");
                    Console.WriteLine("0 - Назад");

                    if (!int.TryParse(Console.ReadLine(), out int uChoice))
                    {
                        Console.WriteLine("Введіть число!");
                        continue;
                    }

                    if (uChoice == 1)
                    {
                        patientService.ShowMedicines(login);
                    }
                    else if (uChoice == 2)
                    {
                        patientService.MarkMedicine(login);
                    }
                    else if (uChoice == 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Невірний вибір");
                    }
                }




                if (user == null)
                {
                    Console.WriteLine("Невірні дані");
                    continue;
                }


                patientService.ShowMedicines(login);
            }

            // РЕЄСТРАЦІЯ
            else if (choice == 3)
            {
                Console.Write("Логін: ");
                string login = Console.ReadLine();

                Console.Write("Пароль: ");
                string pass = Console.ReadLine();

                userService.Register(new User
                {
                    Id = Guid.NewGuid(),
                    Login = login,
                    Password = pass
                });

                Console.WriteLine("Готово");
            }

            else break;
        }
    }
}