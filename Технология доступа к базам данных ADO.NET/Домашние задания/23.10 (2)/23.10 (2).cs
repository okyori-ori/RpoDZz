using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

try
{
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .AddEnvironmentVariables()
        .Build();

    string? connectionString = configuration.GetConnectionString("CoffeeShop");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        Console.WriteLine("Строка подключения CoffeeShop не найдена.");
        return;
    }

    while (true)
    {
        Console.WriteLine("1. Количество клиентов");
        Console.WriteLine("2. Количество товаров");
        Console.WriteLine("3. Количество заказов");
        Console.WriteLine("0. Выход");
        string sql;
        switch (Console.ReadLine())
        {
            case "1": sql = "SELECT COUNT(*) FROM customers;"; break;
            case "2": sql = "SELECT COUNT(*) FROM products;"; break;
            case "3": sql = "SELECT COUNT(*) FROM orders;"; break;
            case "0": return;
            default:
                Console.WriteLine("Нет такого пункта.");
                continue;
        }
        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand command = new SqlCommand(sql, connection);
            int count = Convert.ToInt32(command.ExecuteScalar());
            Console.WriteLine($"Количество записей: {count}");
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка базы данных: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка строки подключения: {ex.Message}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка конфигурации: {ex.Message}");
}
