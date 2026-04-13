using System;

public class Medicine
{
    private string _name;
    private string _dosage;
    private string _time;

    public Guid Id { get; private set; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Назва ліків не може бути порожня");

            if (value.Length > 50) 
                throw new ArgumentException("Назва занадто довга (до 50 символів)");

            _name = value.Trim(); 
        }
    }

    public string Dosage
    {
        get => _dosage;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Дозування не може бути порожнім"); 

            _dosage = value.Trim(); 
        }
    }

    public string Time
    {
        get => _time;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Час не може бути порожнім");

            //  перевірка формату часу
            if (!TimeSpan.TryParse(value, out _))
                throw new ArgumentException("Невірний формат часу (приклад: 08:00)");

            _time = value.Trim(); 
        }
    }

    public DateTime CreatedAt { get; private set; }

    public Medicine(string name, string dosage, string time)
    {
        Id = Guid.NewGuid(); 

        Name = name;    
        Dosage = dosage; 
        Time = time;     

        CreatedAt = DateTime.Now; 
    }

    public override string ToString()
    {
        
        return $"[{CreatedAt:dd.MM.yyyy HH:mm}] {Name} | Дозування: {Dosage} | Час: {Time}";
    }
}