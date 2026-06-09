using System;

abstract class Delivery
{
    public string Address;      
    public string Recipient;   

    public abstract int BaseCost { get; set; }     
    public abstract DateTime SendDateTime { get; set; } 

    public abstract double CalculateCost(int distance, int weight);
    public abstract string GetDeliveryTime();

    public void ShowReceiveDate()
    {

        string timeString = GetDeliveryTime();
        int days = int.Parse(timeString.Split()[0]);

        DateTime receiveDate = SendDateTime.AddDays(days);

        Console.WriteLine($"Дата получения: {receiveDate:dd, MM, yyyy}");
    }
}

class HomeDelivery : Delivery
{
    public override int BaseCost { get; set; }
    public override DateTime SendDateTime { get; set; }

    public HomeDelivery(string address, string recipient, int baseCost, DateTime sendDate)
    {
        Address = address;
        Recipient = recipient;
        BaseCost = baseCost;
        SendDateTime = sendDate;
    }

    public override double CalculateCost(int distance, int weight)
    {
        return distance * weight * BaseCost;
    }

    public override string GetDeliveryTime()
    {
        return "3 дня";
    }
}

class PickPointDelivery : Delivery
{
    public override int BaseCost { get; set; }
    public override DateTime SendDateTime { get; set; }

    public PickPointDelivery(string address, string recipient, int baseCost, DateTime sendDate)
    {
        Address = address;
        Recipient = recipient;
        BaseCost = baseCost;
        SendDateTime = sendDate;
    }

    public override double CalculateCost(int distance, int weight)
    {
        return 200 + (weight * BaseCost);
    }

    public override string GetDeliveryTime()
    {
        return "2 дня";
    }
}

class ExpressDelivery : Delivery
{
    public override int BaseCost { get; set; }
    public override DateTime SendDateTime { get; set; }

    public ExpressDelivery(string address, string recipient, int baseCost, DateTime sendDate)
    {
        Address = address;
        Recipient = recipient;
        BaseCost = baseCost;
        SendDateTime = sendDate;
    }

    public override double CalculateCost(int distance, int weight)
    {
        double normalCost = distance * weight * BaseCost;

        return normalCost * 2;
    }

    public override string GetDeliveryTime()
    {
        return "1 день";
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== СИСТЕМА ДОСТАВКИ ===\n");

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Создать обычную доставку на дом");
            Console.WriteLine("2. Создать доставку в пункт выдачи");
            Console.WriteLine("3. Создать экспресс-доставку");
            Console.WriteLine("4. Показать примеры всех доставок");
            Console.WriteLine("5. Выйти");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateHomeDelivery();
                    break;
                case "2":
                    CreatePickPointDelivery();
                    break;
                case "3":
                    CreateExpressDelivery();
                    break;
                case "4":
                    ShowExamples();
                    break;
                case "5":
                    exit = true;
                    Console.WriteLine("Спасибо за использование системы доставки!");
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    static void CreateHomeDelivery()
    {
        Console.Clear();
        Console.WriteLine("=== СОЗДАНИЕ ОБЫЧНОЙ ДОСТАВКИ НА ДОМ ===\n");

        Console.Write("Введите адрес доставки: ");
        string address = Console.ReadLine();

        Console.Write("Введите имя получателя: ");
        string recipient = Console.ReadLine();

        Console.Write("Введите расстояние (км): ");
        int distance = int.Parse(Console.ReadLine());

        Console.Write("Введите вес посылки (кг): ");
        int weight = int.Parse(Console.ReadLine());

        Console.Write("Введите базовую стоимость доставки 1 кг: ");
        int baseCost = int.Parse(Console.ReadLine());

        DateTime sendDate = DateTime.Now;

        HomeDelivery delivery = new HomeDelivery(address, recipient, baseCost, sendDate);

        Console.WriteLine("\n--- РЕЗУЛЬТАТ ---");
        Console.WriteLine($"Получатель: {delivery.Recipient}");
        Console.WriteLine($"Адрес: {delivery.Address}");
        Console.WriteLine($"Дата отправки: {delivery.SendDateTime:dd.MM.yyyy}");
        Console.WriteLine($"Стоимость доставки: {delivery.CalculateCost(distance, weight)} руб.");
        Console.WriteLine($"Время доставки: {delivery.GetDeliveryTime()}");
        delivery.ShowReceiveDate();

        Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
        Console.ReadLine();
    }

    static void CreatePickPointDelivery()
    {
        Console.Clear();
        Console.WriteLine("=== СОЗДАНИЕ ДОСТАВКИ В ПУНКТ ВЫДАЧИ ===\n");

        Console.Write("Введите адрес пункта выдачи: ");
        string address = Console.ReadLine();

        Console.Write("Введите имя получателя: ");
        string recipient = Console.ReadLine();

        Console.Write("Введите расстояние (км): ");
        int distance = int.Parse(Console.ReadLine()); 

        Console.Write("Введите вес посылки (кг): ");
        int weight = int.Parse(Console.ReadLine());

        Console.Write("Введите базовую стоимость доставки 1 кг: ");
        int baseCost = int.Parse(Console.ReadLine());

        DateTime sendDate = DateTime.Now;

        PickPointDelivery delivery = new PickPointDelivery(address, recipient, baseCost, sendDate);

        Console.WriteLine("\n--- РЕЗУЛЬТАТ ---");
        Console.WriteLine($"Получатель: {delivery.Recipient}");
        Console.WriteLine($"Адрес пункта выдачи: {delivery.Address}");
        Console.WriteLine($"Дата отправки: {delivery.SendDateTime:dd.MM.yyyy}");
        Console.WriteLine($"Стоимость доставки: {delivery.CalculateCost(distance, weight)} руб. (200 руб. + {weight} кг * {baseCost} руб.)");
        Console.WriteLine($"Время доставки: {delivery.GetDeliveryTime()}");
        delivery.ShowReceiveDate();

        Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
        Console.ReadLine();
    }

    static void CreateExpressDelivery()
    {
        Console.Clear();
        Console.WriteLine("=== СОЗДАНИЕ ЭКСПРЕСС-ДОСТАВКИ ===\n");

        Console.Write("Введите адрес доставки: ");
        string address = Console.ReadLine();

        Console.Write("Введите имя получателя: ");
        string recipient = Console.ReadLine();

        Console.Write("Введите расстояние (км): ");
        int distance = int.Parse(Console.ReadLine());

        Console.Write("Введите вес посылки (кг): ");
        int weight = int.Parse(Console.ReadLine());

        Console.Write("Введите базовую стоимость доставки 1 кг: ");
        int baseCost = int.Parse(Console.ReadLine());

        DateTime sendDate = DateTime.Now;

        ExpressDelivery delivery = new ExpressDelivery(address, recipient, baseCost, sendDate);

        Console.WriteLine("\n--- РЕЗУЛЬТАТ ---");
        Console.WriteLine($"Получатель: {delivery.Recipient}");
        Console.WriteLine($"Адрес: {delivery.Address}");
        Console.WriteLine($"Дата отправки: {delivery.SendDateTime:dd.MM.yyyy}");

        double normalCost = distance * weight * baseCost;
        Console.WriteLine($"Обычная стоимость была бы: {normalCost} руб.");
        Console.WriteLine($"Экспресс-стоимость (×2): {delivery.CalculateCost(distance, weight)} руб.");
        Console.WriteLine($"Время доставки: {delivery.GetDeliveryTime()} (обычно 3 дня)");
        delivery.ShowReceiveDate();

        Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
        Console.ReadLine();
    }

    static void ShowExamples()
    {
        Console.Clear();
        Console.WriteLine("=== ПРИМЕРЫ ВСЕХ ТИПОВ ДОСТАВКИ ===\n");

        DateTime sendDate = DateTime.Now;
        int distance = 100;
        int weight = 5;
        int baseCost = 10;

        Console.WriteLine("1. ОБЫЧНАЯ ДОСТАВКА НА ДОМ:");
        HomeDelivery home = new HomeDelivery("ул. Ленина, д.1", "Иван Петров", baseCost, sendDate);
        Console.WriteLine($"   Получатель: {home.Recipient}");
        Console.WriteLine($"   Адрес: {home.Address}");
        Console.WriteLine($"   Стоимость: {home.CalculateCost(distance, weight)} руб.");
        Console.WriteLine($"   Время: {home.GetDeliveryTime()}");
        home.ShowReceiveDate();

        Console.WriteLine("\n2. ДОСТАВКА В ПУНКТ ВЫДАЧИ:");
        PickPointDelivery pickpoint = new PickPointDelivery("ул. Пушкина, д.10 (ПВЗ)", "Мария Сидорова", baseCost, sendDate);
        Console.WriteLine($"   Получатель: {pickpoint.Recipient}");
        Console.WriteLine($"   Адрес: {pickpoint.Address}");
        Console.WriteLine($"   Стоимость: {pickpoint.CalculateCost(distance, weight)} руб. (фикс 200 + вес×{baseCost})");
        Console.WriteLine($"   Время: {pickpoint.GetDeliveryTime()}");
        pickpoint.ShowReceiveDate();

        Console.WriteLine("\n3. ЭКСПРЕСС-ДОСТАВКА:");
        ExpressDelivery express = new ExpressDelivery("пр. Мира, д.5", "Алексей Иванов", baseCost, sendDate);
        double normal = distance * weight * baseCost;
        Console.WriteLine($"   Получатель: {express.Recipient}");
        Console.WriteLine($"   Адрес: {express.Address}");
        Console.WriteLine($"   Обычная стоимость: {normal} руб.");
        Console.WriteLine($"   Экспресс-стоимость: {express.CalculateCost(distance, weight)} руб. (×2)");
        Console.WriteLine($"   Время: {express.GetDeliveryTime()} (в 3 раза быстрее обычного)");
        express.ShowReceiveDate();

        Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
        Console.ReadLine();
    }
}