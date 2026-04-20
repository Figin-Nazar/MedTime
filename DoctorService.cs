public class DoctorService


{
    private UserService userService;
    private EmailService emailService;

    public void SaveAllDoctors()
    {
        
    }

    public DoctorService(UserService userService, EmailService emailService)
    {
        this.userService = userService;
        this.emailService = emailService;
    }

    public void CreateAndSend(string login, string password, string email)
    {
        // 1. створення лікаря
        userService.AddDoctor(login, password, email);

        // 2. відправка
        string subject = "Доступ до системи";
        string body = $"Ваш логін: {login}\nВаш пароль: {password}";

        emailService.Send(email, subject, body);
    }
}