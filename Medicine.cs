using System;

public class Medicine
{
    private string _name;
    private string _dosage;

    public Guid Id { get; private set; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Назва ліків не може бути порожня");
            _name = value;
        }
    }

    public string Dosage
    {
        get => _dosage;
        set => _dosage = value;
    }

    public string Time { get; set; }

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
        return $"[{CreatedAt}] {Name} | Дозування: {Dosage} | Час: {Time}";
    }
}

