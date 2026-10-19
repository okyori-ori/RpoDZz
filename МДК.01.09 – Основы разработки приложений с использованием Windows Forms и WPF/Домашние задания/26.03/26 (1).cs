using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        while (true)
        {
            Console.WriteLine("1. Квадрат из символов");
            Console.WriteLine("2. Проверка палиндрома");
            Console.WriteLine("3. Фильтрация массива");
            Console.WriteLine("4. Веб-сайт");
            Console.WriteLine("5. Журнал");
            Console.WriteLine("6. Магазин");
            Console.WriteLine("0. Выход");
            string choice = Console.ReadLine();
            if (choice == null || choice == "0") return;
            switch (choice)
            {
                case "1":
                    Console.Write("Длина стороны: ");
                    int side;
                    if (!int.TryParse(Console.ReadLine(), out side) || side <= 0)
                    {
                        Console.WriteLine("Введите положительное целое число.");
                        break;
                    }
                    Console.Write("Символ: ");
                    string symbol = Console.ReadLine();
                    if (string.IsNullOrEmpty(symbol) || symbol.Length != 1)
                    {
                        Console.WriteLine("Введите один символ.");
                        break;
                    }
                    DrawSquare(side, symbol[0]);
                    break;
                case "2":
                    Console.Write("Целое число: ");
                    int number;
                    if (!int.TryParse(Console.ReadLine(), out number))
                        Console.WriteLine("Некорректное число.");
                    else
                        Console.WriteLine(IsPalindrome(number) ? "Палиндром" : "Не палиндром");
                    break;
                case "3":
                    int[] original = { 1, 2, 6, -1, 88, 7, 6 };
                    int[] filter = { 6, 88, 7 };
                    Console.WriteLine("Исходный массив: " + string.Join(" ", original));
                    Console.WriteLine("Фильтр: " + string.Join(" ", filter));
                    Console.WriteLine("Результат: " + string.Join(" ", FilterArray(original, filter)));
                    break;
                case "4":
                    Website website = new Website();
                    website.Input();
                    website.Display();
                    Console.Write("Новое описание сайта: ");
                    website.SetDescription(Console.ReadLine() ?? "");
                    Console.WriteLine("Описание через метод: " + website.GetDescription());
                    break;
                case "5":
                    Magazine magazine = new Magazine();
                    magazine.Input();
                    magazine.Display();
                    Console.Write("Новое описание журнала: ");
                    magazine.SetDescription(Console.ReadLine() ?? "");
                    Console.WriteLine("Описание через метод: " + magazine.GetDescription());
                    break;
                case "6":
                    Shop shop = new Shop();
                    shop.Input();
                    shop.Display();
                    Console.Write("Новое описание магазина: ");
                    shop.SetDescription(Console.ReadLine() ?? "");
                    Console.WriteLine("Описание через метод: " + shop.GetDescription());
                    break;
                default: Console.WriteLine("Нет такого пункта."); break;
            }
        }
    }

    static void DrawSquare(int side, char symbol)
    {
        for (int row = 0; row < side; row++)
        {
            for (int column = 0; column < side; column++) Console.Write(symbol);
            Console.WriteLine();
        }
    }

    static bool IsPalindrome(int number)
    {
        if (number < 0) return false;
        string text = number.ToString();
        for (int i = 0; i < text.Length / 2; i++)
            if (text[i] != text[text.Length - 1 - i]) return false;
        return true;
    }

    static int[] FilterArray(int[] original, int[] filter)
    {
        List<int> result = new List<int>();
        foreach (int number in original)
            if (Array.IndexOf(filter, number) == -1) result.Add(number);
        return result.ToArray();
    }
}

class Website
{
    private string name;
    private string path;
    private string description;
    private string ipAddress;

    public void Input()
    {
        Console.Write("Название сайта: ");
        name = Console.ReadLine() ?? "";
        Console.Write("Путь к сайту: ");
        path = Console.ReadLine() ?? "";
        Console.Write("Описание: ");
        description = Console.ReadLine() ?? "";
        Console.Write("IP-адрес: ");
        ipAddress = Console.ReadLine() ?? "";
    }

    public void Display()
    {
        Console.WriteLine($"Название сайта: {name}");
        Console.WriteLine($"Путь к сайту: {path}");
        Console.WriteLine($"Описание: {description}");
        Console.WriteLine($"IP-адрес: {ipAddress}");
    }

    public string GetName() { return name; }
    public void SetName(string value) { name = value; }

    public string GetPath() { return path; }
    public void SetPath(string value) { path = value; }

    public string GetDescription() { return description; }
    public void SetDescription(string value) { description = value; }

    public string GetIpAddress() { return ipAddress; }
    public void SetIpAddress(string value) { ipAddress = value; }
}

class Magazine
{
    private string name;
    private int year;
    private string description;
    private string phone;
    private string email;

    public void Input()
    {
        Console.Write("Название журнала: ");
        name = Console.ReadLine() ?? "";
        Console.Write("Год основания: ");
        while (!int.TryParse(Console.ReadLine(), out year) || year <= 0)
            Console.Write("Введите положительный год: ");
        Console.Write("Описание: ");
        description = Console.ReadLine() ?? "";
        Console.Write("Телефон: ");
        phone = Console.ReadLine() ?? "";
        Console.Write("E-mail: ");
        email = Console.ReadLine() ?? "";
    }

    public void Display()
    {
        Console.WriteLine($"Название журнала: {name}");
        Console.WriteLine($"Год основания: {year}");
        Console.WriteLine($"Описание: {description}");
        Console.WriteLine($"Телефон: {phone}");
        Console.WriteLine($"E-mail: {email}");
    }

    public string GetName() { return name; }
    public void SetName(string value) { name = value; }

    public int GetYear() { return year; }
    public void SetYear(int value) { year = value; }

    public string GetDescription() { return description; }
    public void SetDescription(string value) { description = value; }

    public string GetPhone() { return phone; }
    public void SetPhone(string value) { phone = value; }

    public string GetEmail() { return email; }
    public void SetEmail(string value) { email = value; }
}

class Shop
{
    private string name;
    private string address;
    private string description;
    private string phone;
    private string email;

    public void Input()
    {
        Console.Write("Название магазина: ");
        name = Console.ReadLine() ?? "";
        Console.Write("Адрес: ");
        address = Console.ReadLine() ?? "";
        Console.Write("Описание профиля: ");
        description = Console.ReadLine() ?? "";
        Console.Write("Телефон: ");
        phone = Console.ReadLine() ?? "";
        Console.Write("E-mail: ");
        email = Console.ReadLine() ?? "";
    }

    public void Display()
    {
        Console.WriteLine($"Название магазина: {name}");
        Console.WriteLine($"Адрес: {address}");
        Console.WriteLine($"Описание профиля: {description}");
        Console.WriteLine($"Телефон: {phone}");
        Console.WriteLine($"E-mail: {email}");
    }

    public string GetName() { return name; }
    public void SetName(string value) { name = value; }

    public string GetAddress() { return address; }
    public void SetAddress(string value) { address = value; }

    public string GetDescription() { return description; }
    public void SetDescription(string value) { description = value; }

    public string GetPhone() { return phone; }
    public void SetPhone(string value) { phone = value; }

    public string GetEmail() { return email; }
    public void SetEmail(string value) { email = value; }
}
