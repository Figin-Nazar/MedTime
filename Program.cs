using System.Text;


class Program
{




    private static void Main()
    {
        UserService userService = new UserService();
        DoctorService doctorService = new DoctorService();

        List<User> users = userService.LoadUsers();
        List<User> doctors = doctorService.LoadDoctors();

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        bool Bubier = true;

        while (Bubier)
        {
            Console.WriteLine("1 - Увійти як лікар");
            Console.WriteLine("2 - Увійти як користувач");
            Console.WriteLine("3 - Зареєструватися");
            Console.WriteLine("0 - Вийти");

            int choice = Convert.ToInt32(Console.ReadLine());

            // 🔥 ВХІД ЛІКАР
            if (choice == 1)
            {
                Console.Write("Логін: ");
                string login = Console.ReadLine();

                Console.Write("Пароль: ");
                string password = Console.ReadLine();

                bool found = false;

                foreach (var d in doctors) // ✅ ТУТ doctors
                {
                    if (d.Login == login && d.Password == password)
                    {
                        Console.WriteLine("✅ Вхід як лікар");

                        // 🔥 перший вхід
                        if (d.IsFirstLogin)
                        {
                            Console.Write("Введіть новий пароль: ");
                            string newPassword = Console.ReadLine();

                            d.ChangePassword(newPassword);

                            doctorService.SaveAllDoctors(doctors); // ✅

                            Console.WriteLine("✅ Пароль змінено!");
                        }

                        found = true;
                        break;
                    }
                }

                if (!found)
                    Console.WriteLine("❌ Невірний логін або пароль");
            }

            // 🔥 ВХІД КОРИСТУВАЧА
            else if (choice == 2)
            {
                Console.Write("Логін: ");
                string login = Console.ReadLine();

                Console.Write("Пароль: ");
                string password = Console.ReadLine();

                bool found = false;

                foreach (var u in users)
                {
                    if (u.Login == login && u.Password == password)
                    {
                        Console.WriteLine("✅ Ви користувач");
                        found = true;
                        break;
                    }
                }

                if (!found)
                    Console.WriteLine("❌ Користувача не знайдено");
            }

            // 🔥 РЕЄСТРАЦІЯ
            else if (choice == 3)
            {
                Console.Write("Новий логін: ");
                string login = Console.ReadLine();

                Console.Write("Пароль: ");
                string password = Console.ReadLine();

                bool exists = false;

                foreach (var u in users)
                {
                    if (u.Login == login)
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists)
                {
                    Console.WriteLine("❌ Такий логін вже є");
                }
                else
                {
                    User newUser = new User(login, password, "User");
                    userService.SaveUser(newUser);

                    Console.WriteLine("✅ Зареєстровано!");

                    users = userService.LoadUsers();
                }
            }

            else if (choice == 0)
            {
                Bubier = false;
            }

            Console.WriteLine();
        }
    }
}