using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

string connectionString =
    "Server=localhost;Database=coffee_shop;" +
    "Trusted_Connection=True;TrustServerCertificate=True;";

while (true)
{
    Console.WriteLine("=== АСИНХРОННЫЙ КАТАЛОГ КОФЕЙНИ ===");
    Console.WriteLine("1. Все товары");
    Console.WriteLine("2. Доступные товары");
    Console.WriteLine("3. Поиск по части названия");
    Console.WriteLine("4. Число клиентов");
    Console.WriteLine("5. Изменить доступность товара");
    Console.WriteLine("6. Найти товары по минимальной цене");
    Console.WriteLine("7. Число товаров и отмена через 5 секунд");
    Console.WriteLine("8. Заказы клиента");
    Console.WriteLine("9. Проверить изменение и восстановить доступность");
    Console.WriteLine("0. Выход");
    string? choice = Console.ReadLine();
    if (choice == "0") break;
    try
    {
        switch (choice)
        {
            case "1": await ShowProductsAsync(connectionString); break;
            case "2":
                await PrintProductsAsync(connectionString,
                    "SELECT id, name, price, is_available FROM products WHERE is_available = 1 ORDER BY name;");
                break;
            case "3":
                await PrintProductsAsync(connectionString,
                    "SELECT id, name, price, is_available FROM products WHERE name LIKE @search ORDER BY name;",
                    "@search", "%" + ReadText("Название: ") + "%");
                break;
            case "4":
                await using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM customers;", connection);
                    Console.WriteLine($"Клиентов: {await command.ExecuteScalarAsync()}");
                }
                break;
            case "5":
                int id = ReadInt("ID товара: ");
                string value = ReadText("Доступен (1 - да, 0 - нет): ");
                if (value != "1" && value != "0")
                    Console.WriteLine("Нужно ввести 1 или 0.");
                else
                    await UpdateProductAvailabilityAsync(connectionString, id, value == "1");
                break;
            case "6":
                await ShowProductsFromPriceAsync(connectionString, ReadPrice("Минимальная цена: "));
                break;
            case "7":
                using (CancellationTokenSource source = new CancellationTokenSource())
                {
                    source.CancelAfter(TimeSpan.FromSeconds(5));
                    int count = await GetProductCountAsync(connectionString, source.Token);
                    Console.WriteLine($"Количество товаров: {count}");
                }
                break;
            case "8":
                await GetCustomerOrdersAsync(connectionString, ReadInt("ID клиента: "));
                break;
            case "9":
                await TestAvailabilityAsync(ReadInt("ID товара: "));
                break;
            default: Console.WriteLine("Нет такого пункта."); break;
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Операция отменена.");
    }
    catch (SqlException ex)
    {
        Console.WriteLine($"Ошибка базы данных: {ex.Message}");
    }
}

static async Task<int> GetProductCountAsync(string connectionString,
    CancellationToken token = default)
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync(token);
    using SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM products;", connection);
    return Convert.ToInt32(await command.ExecuteScalarAsync(token));
}

static async Task ShowProductsAsync(string connectionString)
{
    await PrintProductsAsync(connectionString,
        "SELECT id, name, price, is_available FROM products ORDER BY name;");
}

static async Task ShowProductsFromPriceAsync(string connectionString, decimal minPrice)
{
    await PrintProductsAsync(connectionString,
        "SELECT id, name, price FROM products WHERE price >= @min_price ORDER BY price;",
        "@min_price", minPrice);
}

static async Task PrintProductsAsync(string connectionString, string sql,
    string? parameter = null, object? value = null)
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    using SqlCommand command = new SqlCommand(sql, connection);
    if (parameter != null) command.Parameters.AddWithValue(parameter, value!);
    await using SqlDataReader reader = await command.ExecuteReaderAsync();
    if (!reader.HasRows) Console.WriteLine("Товары не найдены.");
    while (await reader.ReadAsync())
    {
        int id = reader.GetInt32(0);
        string name = reader.GetString(1);
        decimal price = reader.GetDecimal(2);
        Console.Write($"{id}. {name} - {price:F2} руб.");
        if (reader.FieldCount == 4)
        {
            bool available = reader.GetBoolean(3);
            Console.Write($" | {(available ? "Есть" : "Нет")}");
        }
        Console.WriteLine();
    }
}

static async Task UpdateProductAvailabilityAsync(string connectionString,
    int productId, bool isAvailable)
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    using SqlCommand command = new SqlCommand(@"
UPDATE products SET is_available = @is_available WHERE id = @id;", connection);
    command.Parameters.AddWithValue("@is_available", isAvailable);
    command.Parameters.AddWithValue("@id", productId);
    int rows = await command.ExecuteNonQueryAsync();
    Console.WriteLine(rows == 0 ? "Товар не найден." : $"Изменено строк: {rows}");
}

async Task TestAvailabilityAsync(int id)
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    using SqlCommand command = new SqlCommand(
        "SELECT is_available FROM products WHERE id = @id;", connection);
    command.Parameters.AddWithValue("@id", id);
    object? value = await command.ExecuteScalarAsync();
    if (value == null)
    {
        Console.WriteLine("Товар не найден.");
        return;
    }
    bool oldValue = (bool)value;
    try
    {
        await UpdateProductAvailabilityAsync(connectionString, id, !oldValue);
        Console.WriteLine($"Временное значение: {!oldValue}");
    }
    finally
    {
        await UpdateProductAvailabilityAsync(connectionString, id, oldValue);
        Console.WriteLine($"Восстановлено прежнее значение: {oldValue}");
    }
    Console.WriteLine($"Проверка в базе: {await command.ExecuteScalarAsync()}");
}

static async Task GetCustomerOrdersAsync(string connectionString, int customerId)
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    using SqlCommand command = new SqlCommand(@"
SELECT o.id, o.order_date, o.status FROM orders o
JOIN customers c ON c.id = o.customer_id
WHERE c.id = @customer_id ORDER BY o.order_date;", connection);
    command.Parameters.AddWithValue("@customer_id", customerId);
    await using SqlDataReader reader = await command.ExecuteReaderAsync();
    if (!reader.HasRows) Console.WriteLine("Заказы не найдены.");
    while (await reader.ReadAsync())
        Console.WriteLine($"{reader.GetInt32(0)} | {reader.GetDateTime(1):dd.MM.yyyy HH:mm} | {reader.GetString(2)}");
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
