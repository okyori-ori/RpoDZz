using System;
using System.Collections.Generic;

class Program
{
    static Dictionary<string, int> shopItems = new Dictionary<string, int>
    {
        {"Health Potion", 50},
        {"Mana Potion", 75},
        {"Sword", 200},
        {"Shield", 150}
    };

    static void Main()
    {
        int choice;

        do
        {
            Console.Clear();
            Console.WriteLine("===== ИГРОВОЙ МАГАЗИН =====");
            Console.WriteLine("1. Показать все товары");
            Console.WriteLine("2. Добавить новый товар");
            Console.WriteLine("3. Удалить товар");
            Console.WriteLine("4. Изменить цену товара");
            Console.WriteLine("5. Подсчитать стоимость покупки");
            Console.WriteLine("6. Выйти");
            Console.Write("Выберите действие (1-6): ");

            string input = Console.ReadLine();

            if (int.TryParse(input, out choice))
            {
                switch (choice)
                {
                    case 1:
                        ShowAllItems();
                        break;
                    case 2:
                        AddItem();
                        break;
                    case 3:
                        RemoveItem();
                        break;
                    case 4:
                        UpdatePrice();
                        break;
                    case 5:
                        CalculateTotal();
                        break;
                    case 6:
                        Console.WriteLine("Спасибо за посещение магазина!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор! Нажмите Enter...");
                        Console.ReadLine();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Ошибка! Введите число от 1 до 6. Нажмите Enter...");
                Console.ReadLine();
            }
        } while (choice != 6);
    }


    static void ShowAllItems()
    {
        Console.Clear();
        Console.WriteLine("===== ТОВАРЫ В МАГАЗИНЕ =====");

        if (shopItems.Count == 0)
        {
            Console.WriteLine("Магазин пуст!");
        }
        else
        {
            int number = 1;
            foreach (KeyValuePair<string, int> item in shopItems)
            {
                Console.WriteLine($"{number}. {item.Key} - {item.Value} монет");
                number++;
            }
        }

        Console.WriteLine("\nНажмите Enter, чтобы вернуться в меню...");
        Console.ReadLine();
    }


    static void AddItem()
    {
        Console.Clear();
        Console.WriteLine("===== ДОБАВЛЕНИЕ ТОВАРА =====");

        Console.Write("Введите название товара: ");
        string name = Console.ReadLine();

 
        if (shopItems.ContainsKey(name))
        {
            Console.WriteLine("Ошибка! Такой товар уже существует!");
            Console.ReadLine();
            return;
        }

        Console.Write("Введите цену товара: ");
        if (int.TryParse(Console.ReadLine(), out int price))
        {
            shopItems.Add(name, price);
            Console.WriteLine($"Товар '{name}' добавлен с ценой {price} монет!");
        }
        else
        {
            Console.WriteLine("Ошибка! Цена должна быть числом.");
        }

        Console.ReadLine();
    }


    static void RemoveItem()
    {
        Console.Clear();
        Console.WriteLine("===== УДАЛЕНИЕ ТОВАРА =====");

        ShowAllItemsQuick();

        Console.Write("Введите название товара для удаления: ");
        string name = Console.ReadLine();

        if (shopItems.ContainsKey(name))
        {
            shopItems.Remove(name);
            Console.WriteLine($"Товар '{name}' удалён из магазина!");
        }
        else
        {
            Console.WriteLine("Ошибка! Товар не найден.");
        }

        Console.ReadLine();
    }

  
    static void UpdatePrice()
    {
        Console.Clear();
        Console.WriteLine("===== ИЗМЕНЕНИЕ ЦЕНЫ =====");

        ShowAllItemsQuick();

        Console.Write("Введите название товара: ");
        string name = Console.ReadLine();

        if (shopItems.ContainsKey(name))
        {
            Console.WriteLine($"Текущая цена товара '{name}': {shopItems[name]} монет");
            Console.Write("Введите новую цену: ");

            if (int.TryParse(Console.ReadLine(), out int newPrice))
            {
                shopItems[name] = newPrice;
                Console.WriteLine($"Цена товара '{name}' изменена на {newPrice} монет!");
            }
            else
            {
                Console.WriteLine("Ошибка! Цена должна быть числом.");
            }
        }
        else
        {
            Console.WriteLine("Ошибка! Товар не найден.");
        }

        Console.ReadLine();
    }


    static void CalculateTotal()
    {
        Console.Clear();
        Console.WriteLine("===== ОФОРМЛЕНИЕ ПОКУПКИ =====");

        ShowAllItemsQuick();

        int total = 0;
        string continueShopping = "да";

        while (continueShopping == "да" || continueShopping == "Да" || continueShopping == "yes")
        {
            Console.Write("\nВведите название товара: ");
            string name = Console.ReadLine();

            if (shopItems.ContainsKey(name))
            {
                Console.Write($"Введите количество '{name}': ");
                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                {
                    int cost = shopItems[name] * quantity;
                    total += cost;
                    Console.WriteLine($"Добавлено: {name} x{quantity} = {cost} монет");
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите положительное число.");
                }
            }
            else
            {
                Console.WriteLine("Ошибка! Товар не найден.");
            }

            Console.Write("Хотите добавить ещё товар? (да/нет): ");
            continueShopping = Console.ReadLine();
        }

        Console.WriteLine($"\nИТОГОВАЯ СТОИМОСТЬ: {total} монет");
        Console.WriteLine("Нажмите Enter, чтобы продолжить...");
        Console.ReadLine();
    }


    static void ShowAllItemsQuick()
    {
        Console.WriteLine("Товары в магазине:");

        if (shopItems.Count == 0)
        {
            Console.WriteLine("Магазин пуст!");
        }
        else
        {
            foreach (KeyValuePair<string, int> item in shopItems)
            {
                Console.WriteLine($"- {item.Key}: {item.Value} монет");
            }
        }

        Console.WriteLine();
    }
}