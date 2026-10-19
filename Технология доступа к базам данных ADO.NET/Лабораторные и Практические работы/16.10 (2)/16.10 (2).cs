using System;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

string connectionString =
    "Server=localhost;Database=coffee_shop;" +
    "Trusted_Connection=True;TrustServerCertificate=True;";

try
{
    using SqlConnection connection = new SqlConnection(connectionString);
    connection.Open();
    while (true)
    {
        Console.WriteLine("=== ЧТЕНИЕ ДАННЫХ ===");
        Console.WriteLine("1. Все товары");
        Console.WriteLine("2. Товар по ID");
        Console.WriteLine("3. Товары по части названия");
        Console.WriteLine("4. Все клиенты");
        Console.WriteLine("5. Все заказы");
        Console.WriteLine("6. Доступные товары по возрастанию цены");
        Console.WriteLine("7. Товары дороже заданной цены");
        Console.WriteLine("8. Клиенты по части ФИО");
        Console.WriteLine("9. Заказы по статусу");
        Console.WriteLine("10. Заказы с именами клиентов");
        Console.WriteLine("0. Выход");
        string? choice = Console.ReadLine();
        if (choice == "0") return;
        string sql;
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        switch (choice)
        {
            case "1":
                sql = "SELECT id, name, price, is_available FROM products ORDER BY id;";
                break;
            case "2":
                sql = "SELECT id, name, price, is_available FROM products WHERE id = @id;";
                command.Parameters.AddWithValue("@id", ReadInt("ID: "));
                break;
            case "3":
                sql = "SELECT id, name, price FROM products WHERE name LIKE @search;";
                command.Parameters.AddWithValue("@search", "%" + ReadText("Название: ") + "%");
                break;
            case "4":
                sql = "SELECT id, full_name, phone, email FROM customers ORDER BY id;";
                break;
            case "5":
                sql = "SELECT id, customer_id, order_date, status FROM orders ORDER BY id;";
                break;
            case "6":
                sql = @"SELECT id, name, price, is_available FROM products
WHERE is_available = 1 ORDER BY price, id;";
                break;
            case "7":
                sql = @"SELECT id, name, price, is_available FROM products
WHERE price > @price ORDER BY price, id;";
                command.Parameters.AddWithValue("@price", ReadPrice("Цена: "));
                break;
            case "8":
                sql = @"SELECT id, full_name, phone, email FROM customers
WHERE full_name LIKE @search;";
                command.Parameters.AddWithValue("@search", "%" + ReadText("ФИО: ") + "%");
                break;
            case "9":
                sql = "SELECT id, customer_id, order_date, status FROM orders WHERE status = @status;";
                command.Parameters.AddWithValue("@status", ReadText("Статус: "));
                break;
            case "10":
                sql = @"SELECT o.id, c.full_name, o.order_date, o.status
FROM orders o LEFT JOIN customers c ON c.id = o.customer_id ORDER BY o.id;";
                break;
            default:
                Console.WriteLine("Нет такого пункта.");
                continue;
        }
        command.CommandText = sql;
        try
        {
            using SqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows) Console.WriteLine("Записи не найдены.");
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                if (choice == "4" || choice == "8")
                {
                    string phone = reader.IsDBNull(2) ? "Не указан" : reader.GetString(2);
                    string email = reader.IsDBNull(3) ? "Не указан" : reader.GetString(3);
                    Console.WriteLine($"{id}. {reader.GetString(1)} | {phone} | {email}");
                }
                else if (choice == "5" || choice == "9" || choice == "10")
                {
                    string customer = reader.IsDBNull(1) ? "Не указан"
                        : choice == "10" ? reader.GetString(1)
                        : reader.GetInt32(1).ToString();
                    DateTime date = reader.GetDateTime(2);
                    string status = reader.GetString(3);
                    Console.WriteLine($"{id}. {customer} | {date:dd.MM.yyyy HH:mm} | {status}");
                }
                else
                {
                    string name = reader.GetString(1);
                    decimal price = reader.GetDecimal(2);
                    Console.WriteLine($"{id}. {name} | {price:F2} руб.");
                    if (choice != "3")
                    {
                        bool available = reader.GetBoolean(3);
                        Console.WriteLine($"Доступен: {(available ? "Да" : "Нет")}");
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка базы данных: {ex.Message}");
        }
    }
}
catch (SqlException ex)
{
    Console.WriteLine($"Ошибка подключения: {ex.Message}");
}

int ReadInt(string prompt)
{
    Console.Write(prompt);
    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
            return value;
        Console.Write("Введите положительное целое число: ");
    }
}

decimal ReadPrice(string prompt)
{
    Console.Write(prompt);
    while (true)
    {
        string input = (Console.ReadLine() ?? "").Trim().Replace(',', '.');
        if (decimal.TryParse(input, NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out decimal value) && value >= 0)
            return value;
        Console.Write("Введите неотрицательное число: ");
    }
}

string ReadText(string prompt)
{
    Console.Write(prompt);
    string value = (Console.ReadLine() ?? "").Trim();
    while (value.Length == 0)
    {
        Console.Write("Введите непустую строку: ");
        value = (Console.ReadLine() ?? "").Trim();
    }
    return value;
}
