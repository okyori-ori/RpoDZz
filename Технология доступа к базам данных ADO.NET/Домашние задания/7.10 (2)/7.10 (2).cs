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
    Console.WriteLine("Соединение с базой данных установлено.");
    Console.WriteLine("=== Информация о базе данных ===");

    using SqlCommand database = new SqlCommand("SELECT DB_NAME();", connection);
    Console.WriteLine($"База данных: {database.ExecuteScalar()}");

    using SqlCommand server = new SqlCommand("SELECT @@SERVERNAME;", connection);
    Console.WriteLine($"SQL Server: {server.ExecuteScalar()}");

    using SqlCommand date = new SqlCommand("SELECT GETDATE();", connection);
    Console.WriteLine($"Дата и время сервера: {date.ExecuteScalar()}");

    using SqlCommand count = new SqlCommand("SELECT COUNT(*) FROM products;", connection);
    Console.WriteLine($"Количество товаров: {count.ExecuteScalar()}");

    using SqlCommand min = new SqlCommand("SELECT MIN(price) FROM products;", connection);
    object? minPrice = min.ExecuteScalar();
    Console.WriteLine(minPrice == null || minPrice == DBNull.Value
        ? "Минимальная цена: товаров нет"
        : $"Минимальная цена: {minPrice}");

    using SqlCommand max = new SqlCommand("SELECT MAX(price) FROM products;", connection);
    object? maxPrice = max.ExecuteScalar();
    Console.WriteLine(maxPrice == null || maxPrice == DBNull.Value
        ? "Максимальная цена: товаров нет"
        : $"Максимальная цена: {maxPrice}");

    using SqlCommand expensive = new SqlCommand(
        "SELECT TOP 1 name FROM products ORDER BY price DESC, id;", connection);
    object? productName = expensive.ExecuteScalar();
    Console.WriteLine(productName == null
        ? "Самый дорогой товар: товаров нет"
        : $"Самый дорогой товар: {productName}");
}
catch (SqlException ex)
{
    Console.WriteLine($"Ошибка базы данных: {ex.Message}");
}
