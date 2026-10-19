using System;
using System.Globalization;

abstract class Delivery
{
    public string Address;
    public string Recipient;
    public abstract int baseCost { get; }
    public abstract DateTime SendDate { get; set; }

    protected Delivery(string address, string recipient)
    {
        Address = address;
        Recipient = recipient;
    }

    public abstract double CalculateCost();
    public abstract TimeSpan GetDeliveryTime();
}

class HomeDelivery : Delivery
{
    public double Distance;
    public double Weight;
    public override int baseCost { get { return 50; } }
    public override DateTime SendDate { get; set; }

    public HomeDelivery(string address, string recipient, double distance,
        double weight, DateTime sendDate) : base(address, recipient)
    {
        Distance = distance;
        Weight = weight;
        SendDate = sendDate;
    }

    public override double CalculateCost()
    {
        return Distance * 10 + Weight * baseCost;
    }

    public override TimeSpan GetDeliveryTime()
    {
        return TimeSpan.FromDays(3 + Distance / 100);
    }
}

class PickPointDelivery : Delivery
{
    public double Weight;
    public override int baseCost { get { return 30; } }
    public override DateTime SendDate { get; set; }

    public PickPointDelivery(string address, string recipient, double weight,
        DateTime sendDate) : base(address, recipient)
    {
        Weight = weight;
        SendDate = sendDate;
    }

    public override double CalculateCost()
    {
        return 100 + Weight * baseCost;
    }

    public override TimeSpan GetDeliveryTime()
    {
        return TimeSpan.FromDays(3);
    }
}

class ExpressDelivery : HomeDelivery
{
    public ExpressDelivery(string address, string recipient, double distance,
        double weight, DateTime sendDate) : base(address, recipient, distance, weight, sendDate)
    {
    }

    public override double CalculateCost()
    {
        return base.CalculateCost() * 2;
    }

    public override TimeSpan GetDeliveryTime()
    {
        return TimeSpan.FromTicks(base.GetDeliveryTime().Ticks / 3);
    }
}

interface IUserInterface
{
    void ShowDelivery(Delivery delivery);
}

class ConsoleInterface : IUserInterface
{
    public void ShowDelivery(Delivery delivery)
    {
        Console.WriteLine(delivery.GetType().Name);
        Console.WriteLine($"Получатель: {delivery.Recipient}, адрес: {delivery.Address}");
        Console.WriteLine($"Тариф за килограмм: {delivery.baseCost}");
        Console.WriteLine($"Стоимость: {delivery.CalculateCost():F2}");
        Console.WriteLine($"Срок в днях: {delivery.GetDeliveryTime().TotalDays:F2}");
        DateTime receiveDate = delivery.SendDate.Add(delivery.GetDeliveryTime());
        Console.WriteLine($"Дата отправки: {delivery.SendDate:dd.MM.yyyy}");
        Console.WriteLine($"Дата получения: {receiveDate:dd.MM.yyyy}");
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Write("Адрес: ");
        string address = Console.ReadLine() ?? "";
        Console.Write("Получатель: ");
        string recipient = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(recipient))
        {
            Console.WriteLine("Адрес и получатель обязательны.");
            return;
        }
        double distance = ReadNumber("Расстояние в километрах: ");
        double weight = ReadNumber("Вес в килограммах: ");
        Console.Write("Дата отправки (дд.мм.гггг): ");
        DateTime sendDate;
        while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out sendDate))
            Console.Write("Введите дату в формате дд.мм.гггг: ");

        Delivery[] deliveries =
        {
            new HomeDelivery(address, recipient, distance, weight, sendDate),
            new PickPointDelivery(address, recipient, weight, sendDate),
            new ExpressDelivery(address, recipient, distance, weight, sendDate)
        };
        IUserInterface userInterface = new ConsoleInterface();
        foreach (Delivery delivery in deliveries)
            userInterface.ShowDelivery(delivery);
    }

    static double ReadNumber(string prompt)
    {
        Console.Write(prompt);
        while (true)
        {
            string input = (Console.ReadLine() ?? "").Replace(',', '.');
            double value;
            if (double.TryParse(input, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out value) && value > 0)
                return value;
            Console.Write("Введите положительное число: ");
        }
    }
}
