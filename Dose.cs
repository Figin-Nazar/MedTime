class Dose
{
    private double _amount;

    public string Unit { get; private set; } // mg, ml, таблетка

    public double Amount
    {
        get => _amount;
        set
        {
            if (value <= 0) 
                throw new ArgumentException("Кількість повинна бути більше 0");

            if (value > 10000) 
                throw new ArgumentException("Занадто велика доза");

            _amount = value;
        }
    }

    public Dose(string unit, double amount)
    {
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Одиниця не може бути пустою");

        Unit = unit.Trim();
        Amount = amount;
    }
}