using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

string connectionString =
    "Server=localhost;Database=coffee_shop;" +
    "Trusted_Connection=True;TrustServerCertificate=True;";

try
{
    int count = await GetAvailableProductCountAsync();
    Console.WriteLine($"Доступных товаров: {count}");

    Console.Write("Введите ID товара: ");
    if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
    {
        Console.WriteLine("Введите положительное целое число.");
        return;
    }
    await FindProductByIdAsync(id);
}
catch (SqlException ex)
{
    Console.WriteLine($"Ошибка базы данных: {ex.Message}");
}

async Task<int> GetAvailableProductCountAsync()
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    using SqlCommand command = new SqlCommand(
        "SELECT COUNT(*) FROM products WHERE is_available = 1;", connection);
    return Convert.ToInt32(await command.ExecuteScalarAsync());
}

async Task FindProductByIdAsync(int id)
{
    await using SqlConnection connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    using SqlCommand command = new SqlCommand(
        "SELECT id, name, price, is_available FROM products WHERE id = @id;", connection);
    command.Parameters.AddWithValue("@id", id);
    await using SqlDataReader reader = await command.ExecuteReaderAsync();
    if (await reader.ReadAsync())
    {
        int productId = reader.GetInt32(0);
        string name = reader.GetString(1);
        decimal price = reader.GetDecimal(2);
        bool available = reader.GetBoolean(3);
        Console.WriteLine($"{productId}. {name} - {price:F2} руб.");
        Console.WriteLine($"Доступен: {(available ? "Да" : "Нет")}");
    }
    else
    {
        Console.WriteLine("Товар не найден.");
    }
}
