using System;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

Console.Write("Введите минимальную цену: ");
string input = (Console.ReadLine() ?? "").Replace(',', '.');

if (!decimal.TryParse(input, NumberStyles.AllowDecimalPoint,
    CultureInfo.InvariantCulture, out decimal minPrice) || minPrice < 0)
{
    Console.WriteLine("Введите неотрицательную цену числом.");
    return;
}

string connectionString =
    "Server=localhost;Database=coffee_shop;" +
    "Trusted_Connection=True;TrustServerCertificate=True;";

try
{
    using SqlConnection connection = new SqlConnection(connectionString);
    connection.Open();

    string sql = @"SELECT id, name, price, is_available
FROM products
WHERE is_available = 1 AND price > @minPrice
ORDER BY id;";

    using SqlCommand command = new SqlCommand(sql, connection);
    command.Parameters.Add("@minPrice", SqlDbType.Decimal).Value = minPrice;
    using SqlDataReader reader = command.ExecuteReader();

    if (!reader.HasRows)
    {
        Console.WriteLine("Доступные товары дороже заданной цены не найдены.");
    }
    else
    {
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.IsDBNull(1)
                ? "Без названия" : reader.GetString(1);
            decimal price = reader.GetDecimal(2);
            bool isAvailable = reader.GetBoolean(3);

            Console.WriteLine($"{id}. {name} | Цена: {price:F2} руб.");
            Console.WriteLine($"Доступность: {isAvailable}");
        }
    }
}
catch (SqlException ex)
{
    Console.WriteLine($"Ошибка базы данных: {ex.Message}");
}
