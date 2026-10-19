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
    Console.WriteLine("Подключение к базе данных установлено.");
    Console.WriteLine($"База данных: {Scalar("SELECT DB_NAME();", connection)}");
    Console.WriteLine($"Количество товаров: {Scalar("SELECT COUNT(*) FROM products;", connection)}");
    Products(connection);

    using SqlCommand add = new SqlCommand(@"
INSERT INTO products (name, category_id, price)
VALUES (N'Мокка', 1, 240.00);
SELECT @new_id = CAST(SCOPE_IDENTITY() AS int);", connection);
    SqlParameter newId = add.Parameters.Add("@new_id", SqlDbType.Int);
    newId.Direction = ParameterDirection.Output;
    Console.WriteLine($"Добавлено строк: {add.ExecuteNonQuery()}");
    int testId = (int)newId.Value;

    using SqlCommand update = new SqlCommand(
        "UPDATE products SET price = 250.00 WHERE id = @id;", connection);
    update.Parameters.AddWithValue("@id", testId);
    Console.WriteLine($"Изменено строк: {update.ExecuteNonQuery()}");
    using (SqlCommand check = new SqlCommand(
        "SELECT price FROM products WHERE id = @id;", connection))
    {
        check.Parameters.AddWithValue("@id", testId);
        Console.WriteLine($"Новая цена Мокка: {check.ExecuteScalar()}");
    }

    object? maximum = Scalar("SELECT MAX(price) FROM products;", connection);
    if (maximum != null && maximum != DBNull.Value)
    {
        decimal maxPrice = Convert.ToDecimal(maximum);
        Console.WriteLine($"Максимальная цена: {maxPrice}");
    }
    Console.WriteLine($"Количество заказов: {Scalar("SELECT COUNT(*) FROM orders;", connection)}");

    using SqlCommand delete = new SqlCommand(
        "DELETE FROM products WHERE id = @id;", connection);
    delete.Parameters.AddWithValue("@id", testId);
    Console.WriteLine($"Удалено строк: {delete.ExecuteNonQuery()}");
    using (SqlCommand check = new SqlCommand(
        "SELECT COUNT(*) FROM products WHERE id = @id;", connection))
    {
        check.Parameters.AddWithValue("@id", testId);
        Console.WriteLine($"Осталось тестовых товаров: {check.ExecuteScalar()}");
    }

    Console.WriteLine($"Клиентов: {Scalar("SELECT COUNT(*) FROM customers;", connection)}");
    Console.WriteLine($"Доступных товаров: {Scalar("SELECT COUNT(*) FROM products WHERE is_available = 1;", connection)}");
    Console.WriteLine($"Минимальная цена: {Scalar("SELECT MIN(price) FROM products;", connection)}");
    Console.WriteLine($"Выданных заказов: {Scalar("SELECT COUNT(*) FROM orders WHERE status = N'Выдан';", connection)}");
    using SqlCommand own = new SqlCommand(
        "INSERT INTO products (name, category_id, price) VALUES (N'Раф', 1, 280.00);", connection);
    Console.WriteLine($"Добавлено строк (Раф): {own.ExecuteNonQuery()}");

    while (true)
    {
        Console.WriteLine("=== COFFEE SHOP ===");
        Console.WriteLine("1. Количество товаров");
        Console.WriteLine("2. Список товаров");
        Console.WriteLine("3. Количество клиентов");
        Console.WriteLine("4. Количество заказов");
        Console.WriteLine("5. Самый дорогой товар");
        Console.WriteLine("6. Добавить товар");
        Console.WriteLine("0. Выход");
        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine(Scalar("SELECT COUNT(*) FROM products;", connection));
                break;
            case "2": Products(connection); break;
            case "3":
                Console.WriteLine(Scalar("SELECT COUNT(*) FROM customers;", connection));
                break;
            case "4":
                Console.WriteLine(Scalar("SELECT COUNT(*) FROM orders;", connection));
                break;
            case "5":
                object? price = Scalar("SELECT MAX(price) FROM products;", connection);
                Console.WriteLine(price == DBNull.Value ? "Товаров нет." : price);
                break;
            case "6":
                string name = ReadText("Название: ");
                int category = ReadInt("ID категории: ");
                decimal cost = ReadPrice("Цена: ");
                using (SqlCommand command = new SqlCommand(@"
INSERT INTO products (name, category_id, price)
VALUES (@name, @category, @price);", connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@category", category);
                    command.Parameters.AddWithValue("@price", cost);
                    Console.WriteLine($"Добавлено строк: {command.ExecuteNonQuery()}");
                }
                break;
            case "0": return;
            default: Console.WriteLine("Нет такого пункта."); break;
        }
    }
}
catch (SqlException ex)
{
    Console.WriteLine($"Ошибка базы данных: {ex.Message}");
}

object? Scalar(string sql, SqlConnection connection)
{
    using SqlCommand command = new SqlCommand(sql, connection);
    return command.ExecuteScalar();
}

void Products(SqlConnection connection)
{
    using SqlCommand command = new SqlCommand(
        "SELECT id, name, price FROM products ORDER BY id;", connection);
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
