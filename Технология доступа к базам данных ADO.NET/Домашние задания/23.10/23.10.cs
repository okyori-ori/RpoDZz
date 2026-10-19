using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

try
{
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile("appsettings.Development.json", optional: true)
        .AddEnvironmentVariables()
        .Build();
    string? connectionString = configuration.GetConnectionString("CoffeeShop");
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Строка подключения CoffeeShop не найдена.");
    string title = configuration["Application:Title"] ?? "Приложение";
    if (!bool.TryParse(configuration["Application:ShowPrices"], out bool showPrices))
        throw new InvalidOperationException("Настройка Application:ShowPrices должна быть true или false.");
    Console.WriteLine(title);

    while (true)
    {
        Console.WriteLine("=== КОНСОЛЬ КОФЕЙНИ ===");
        Console.WriteLine("1. Проверить подключение");
        Console.WriteLine("2. Статистика кофейни");
        Console.WriteLine("3. Доступные товары");
        Console.WriteLine("4. Поиск товаров по названию");
        Console.WriteLine("5. Настройки приложения");
        Console.WriteLine("0. Выход");
        string? choice = Console.ReadLine();
        if (choice == "0") break;
        try
        {
            switch (choice)
            {
                case "1": await CheckConnectionAsync(connectionString); break;
                case "2": await ShowStatisticsAsync(connectionString); break;
                case "3":
                    await ShowCatalogAsync(connectionString, showPrices);
                    break;
                case "4":
                    Console.Write("Часть названия: ");
                    string search = (Console.ReadLine() ?? "").Trim();
                    if (search.Length == 0)
                        Console.WriteLine("Введите непустую строку.");
                    else
                        await ShowCatalogAsync(connectionString, showPrices, search);
                    break;
                case "5":
                    Console.WriteLine($"Название: {title}");
                    Console.WriteLine($"Показывать цены: {showPrices}");
                    break;
                default: Console.WriteLine("Нет такого пункта."); break;
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка подключения или SQL-запроса: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка параметров подключения: {ex.Message}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка конфигурации: {ex.Message}");
}

static async Task CheckConnectionAsync(string connectionString)
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    using SqlCommand command = new SqlCommand("SELECT DB_NAME();", connection);
    string database = Convert.ToString(await command.ExecuteScalarAsync())!;
    Console.WriteLine($"Подключено к базе: {database}");
}

static async Task ShowStatisticsAsync(string connectionString)
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    Console.WriteLine("Статистика кофейни");
    using SqlCommand products = new SqlCommand("SELECT COUNT(*) FROM products;", connection);
    Console.WriteLine($"Товаров: {await products.ExecuteScalarAsync()}");
    using SqlCommand customers = new SqlCommand("SELECT COUNT(*) FROM customers;", connection);
    Console.WriteLine($"Клиентов: {await customers.ExecuteScalarAsync()}");
    using SqlCommand orders = new SqlCommand("SELECT COUNT(*) FROM orders;", connection);
    Console.WriteLine($"Заказов: {await orders.ExecuteScalarAsync()}");
}

static async Task ShowCatalogAsync(string connectionString, bool showPrices,
    string? search = null)
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    string sql = search == null
        ? "SELECT name, price FROM products WHERE is_available = 1 ORDER BY name;"
        : "SELECT name, price FROM products WHERE name LIKE @search ORDER BY name;";
    using SqlCommand command = new SqlCommand(sql, connection);
    if (search != null) command.Parameters.AddWithValue("@search", "%" + search + "%");
    await using SqlDataReader reader = await command.ExecuteReaderAsync();
    if (!reader.HasRows) Console.WriteLine("Товары не найдены.");
    while (await reader.ReadAsync())
    {
        string name = reader.GetString(0);
        if (showPrices)
            Console.WriteLine($"{name} - {reader.GetDecimal(1):F2} руб.");
        else
            Console.WriteLine(name);
    }
}
