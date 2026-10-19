using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Dictionary<string, int> shopItems = new Dictionary<string, int>()
        {
            { "Health Potion", 50 },
            { "Mana Potion", 75 },
            { "Sword", 200 },
            { "Shield", 150 }
        };

        while (true)
        {
            Console.WriteLine("1. Показать товары");
            Console.WriteLine("2. Заменить товар");
            Console.WriteLine("3. Добавить товар");
            Console.WriteLine("4. Удалить товар");
            Console.WriteLine("5. Рассчитать стоимость");
            Console.WriteLine("0. Выход");
            string action = Console.ReadLine();
            if (action == null || action == "0") return;
            switch (action)
            {
                case "1":
                    foreach (KeyValuePair<string, int> item in shopItems)
                        Console.WriteLine($"{item.Key}: {item.Value}");
                    break;
                case "2":
                    Console.Write("Название заменяемого товара: ");
                    string oldName = Console.ReadLine() ?? "";
                    if (!shopItems.ContainsKey(oldName))
                    {
                        Console.WriteLine("Товар не найден.");
                        break;
                    }
                    Console.Write("Новое название: ");
                    string newName = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(newName) ||
                        (newName != oldName && shopItems.ContainsKey(newName)))
                    {
                        Console.WriteLine("Название пустое или уже занято.");
                        break;
                    }
                    int newPrice = ReadNumber("Новая цена: ", false);
                    shopItems.Remove(oldName);
                    shopItems.Add(newName, newPrice);
                    Console.WriteLine("Товар заменён.");
                    break;
                case "3":
                    Console.Write("Название: ");
                    string name = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(name) || shopItems.ContainsKey(name))
                    {
                        Console.WriteLine("Название пустое или уже занято.");
                        break;
                    }
                    shopItems.Add(name, ReadNumber("Цена: ", false));
                    Console.WriteLine("Товар добавлен.");
                    break;
                case "4":
                    Console.Write("Название: ");
                    string removeName = Console.ReadLine() ?? "";
                    Console.WriteLine(shopItems.Remove(removeName) ? "Товар удалён." : "Товар не найден.");
                    break;
                case "5":
                    Console.Write("Название: ");
                    string product = Console.ReadLine() ?? "";
                    if (!shopItems.ContainsKey(product))
                    {
                        Console.WriteLine("Товар не найден.");
                        break;
                    }
                    int quantity = ReadNumber("Количество: ", true);
                    long total = (long)shopItems[product] * quantity;
                    Console.WriteLine($"Стоимость: {total}");
                    break;
                default: Console.WriteLine("Неверная команда."); break;
            }
        }
    }

    static int ReadNumber(string prompt, bool positive)
    {
        Console.Write(prompt);
        int value;
        while (!int.TryParse(Console.ReadLine(), out value) || value < 0 || (positive && value == 0))
            Console.Write("Введите корректное целое число: ");
        return value;
    }
}
