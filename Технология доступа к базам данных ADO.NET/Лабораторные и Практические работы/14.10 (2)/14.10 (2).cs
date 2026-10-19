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
    Console.WriteLine("Подключение установлено.");
    while (true)
    {
        Console.WriteLine("=== УПРАВЛЕНИЕ ТОВАРАМИ ===");
        Console.WriteLine("1. Найти товар по ID");
        Console.WriteLine("2. Найти товар по названию");
        Console.WriteLine("3. Добавить товар");
        Console.WriteLine("4. Изменить цену");
        Console.WriteLine("5. Удалить товар");
        Console.WriteLine("6. Найти товары дороже указанной цены");
        Console.WriteLine("7. Посчитать товары с ценой от указанной");
        Console.WriteLine("8. Найти клиентов по имени");
        Console.WriteLine("0. Выход");
        string? choice = Console.ReadLine();
        if (choice == "0") return;
        try
        {
            switch (choice)
            {
                case "1":
                    using (SqlCommand command = new SqlCommand(
                        "SELECT id, name, price FROM products WHERE id = @id;", connection))
                    {
                        command.Parameters.AddWithValue("@id", ReadInt("ID: "));
                        PrintProducts(command);
                    }
                    break;
                case "2":
                    using (SqlCommand command = new SqlCommand(
                        "SELECT id, name, price FROM products WHERE name = @name;", connection))
                    {
                        command.Parameters.AddWithValue("@name", ReadText("Название: "));
                        PrintProducts(command);
                    }
                    break;
                case "3":
                    string name = ReadText("Название: ");
                    int category = ReadInt("ID категории: ");
                    decimal price = ReadPrice("Цена: ");
                    using (SqlCommand command = new SqlCommand(@"
INSERT INTO products (name, category_id, price)
VALUES (@name, @category_id, @price);", connection))
                    {
                        command.Parameters.Add("@name", SqlDbType.NVarChar, 150).Value = name;
                        command.Parameters.Add("@category_id", SqlDbType.Int).Value = category;
                        SqlParameter parameter = command.Parameters.Add("@price", SqlDbType.Decimal);
                        parameter.Precision = 10;
                        parameter.Scale = 2;
                        parameter.Value = price;
                        Console.WriteLine($"Добавлено строк: {command.ExecuteNonQuery()}");
                    }
                    break;
                case "4":
                    using (SqlCommand command = new SqlCommand(
                        "UPDATE products SET price = @price WHERE id = @id;", connection))
                    {
                        command.Parameters.AddWithValue("@id", ReadInt("ID товара: "));
                        command.Parameters.AddWithValue("@price", ReadPrice("Новая цена: "));
                        Console.WriteLine($"Изменено строк: {command.ExecuteNonQuery()}");
                    }
                    break;
                case "5":
                    using (SqlCommand command = new SqlCommand(
                        "DELETE FROM products WHERE id = @id;", connection))
                    {
                        command.Parameters.AddWithValue("@id", ReadInt("ID товара: "));
                        Console.WriteLine($"Удалено строк: {command.ExecuteNonQuery()}");
                    }
                    break;
                case "6":
                    using (SqlCommand command = new SqlCommand(
                        "SELECT id, name, price FROM products WHERE price > @min_price;", connection))
                    {
                        command.Parameters.AddWithValue("@min_price", ReadPrice("Цена: "));
                        PrintProducts(command);
                    }
                    break;
                case "7":
                    using (SqlCommand command = new SqlCommand(
                        "SELECT COUNT(*) FROM products WHERE price >= @min_price;", connection))
                    {
                        command.Parameters.AddWithValue("@min_price", ReadPrice("Цена от: "));
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        Console.WriteLine($"Количество товаров: {count}");
                    }
                    break;
                case "8":
                    using (SqlCommand command = new SqlCommand(@"
SELECT id, full_name, phone, email FROM customers
WHERE full_name LIKE @search;", connection))
                    {
                        command.Parameters.AddWithValue("@search", "%" + ReadText("Имя: ") + "%");
                        using SqlDataReader reader = command.ExecuteReader();
                        if (!reader.HasRows) Console.WriteLine("Клиенты не найдены.");
                        while (reader.Read())
                        {
                            string phone = reader.IsDBNull(2) ? "Не указан" : reader.GetString(2);
                            string email = reader.IsDBNull(3) ? "Не указан" : reader.GetString(3);
                            Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)} | {phone} | {email}");
                        }
                    }
                    break;
                default: Console.WriteLine("Нет такого пункта."); break;
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

void PrintProducts(SqlCommand command)
{
    using SqlDataReader reader = command.ExecuteReader();
    if (!reader.HasRows) Console.WriteLine("Товары не найдены.");
    while (reader.Read())
        Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)} - {reader.GetDecimal(2):F2}");
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
