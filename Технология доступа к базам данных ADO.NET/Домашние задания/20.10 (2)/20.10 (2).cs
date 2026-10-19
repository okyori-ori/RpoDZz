using System;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

string connectionString =
    "Server=localhost;Database=coffee_shop;" +
    "Trusted_Connection=True;TrustServerCertificate=True;";

HashSet<int> testOrders = new HashSet<int>();
while (true)
{
    Console.WriteLine("=== ТРАНЗАКЦИИ ===");
    Console.WriteLine("1. Изменить цены двух товаров");
    Console.WriteLine("2. Проверить откат изменения цен");
    Console.WriteLine("3. Создать заказ с одной позицией");
    Console.WriteLine("4. Создать заказ с двумя позициями");
    Console.WriteLine("5. Проверить откат после создания заказа");
    Console.WriteLine("6. Удалить свой тестовый заказ");
    Console.WriteLine("7. Проверить откат удаления своего тестового заказа");
    Console.WriteLine("8. Список клиентов и товаров");
    Console.WriteLine("9. Заказы и позиции");
    Console.WriteLine("0. Выход");
    string? choice = Console.ReadLine();
    if (choice == "0") break;
    try
    {
        switch (choice)
        {
            case "1": ChangePrices(false); break;
            case "2": ChangePrices(true); break;
            case "3": CreateOrder(1, false); break;
            case "4": CreateOrder(2, false); break;
            case "5": CreateOrder(1, true); break;
            case "6": DeleteOrder(false); break;
            case "7": DeleteOrder(true); break;
            case "8":
                Show("SELECT id, full_name FROM customers ORDER BY id;");
                Show("SELECT id, name, price FROM products ORDER BY id;");
                break;
            case "9":
                Show(@"SELECT o.id, o.order_date, o.status, oi.product_id,
oi.quantity, oi.unit_price FROM orders o
JOIN order_items oi ON oi.order_id = o.id ORDER BY o.id DESC;");
                break;
            default: Console.WriteLine("Нет такого пункта."); break;
        }
    }
    catch (SqlException ex)
    {
        Console.WriteLine($"Ошибка базы данных: {ex.Message}");
    }
}

void ChangePrices(bool forceError)
{
    int first = ReadInt("ID первого товара: ");
    int second = ReadInt("ID второго товара: ");
    if (first == second)
    {
        Console.WriteLine("Выберите два разных товара.");
        return;
    }
    decimal firstPrice = ReadPrice("Новая цена первого товара: ");
    decimal secondPrice = forceError ? -1 : ReadPrice("Новая цена второго товара: ");
    ShowPrices(first, second);
    using SqlConnection connection = new SqlConnection(connectionString);
    connection.Open();
    using SqlTransaction transaction = connection.BeginTransaction();
    try
    {
        using SqlCommand one = new SqlCommand(
            "UPDATE products SET price = @price WHERE id = @id;", connection, transaction);
        one.Parameters.AddWithValue("@price", firstPrice);
        one.Parameters.AddWithValue("@id", first);
        if (one.ExecuteNonQuery() != 1)
            throw new InvalidOperationException("Первый товар не найден.");

        using SqlCommand two = new SqlCommand(
            "UPDATE products SET price = @price WHERE id = @id;", connection, transaction);
        two.Parameters.AddWithValue("@price", secondPrice);
        two.Parameters.AddWithValue("@id", second);
        if (two.ExecuteNonQuery() != 1)
            throw new InvalidOperationException("Второй товар не найден.");
        transaction.Commit();
        Console.WriteLine("Обе цены сохранены.");
    }
    catch (Exception ex)
    {
        Rollback(transaction);
        Console.WriteLine($"Цены не сохранены: {ex.Message}");
    }
    ShowPrices(first, second);
}

void CreateOrder(int itemCount, bool forceError)
{
    Show("SELECT id, full_name FROM customers ORDER BY id;");
    Show("SELECT id, name, price FROM products ORDER BY id;");
    int customer = ReadInt("ID клиента: ");
    int[] products = new int[itemCount];
    int[] quantities = new int[itemCount];
    for (int i = 0; i < itemCount; i++)
    {
        products[i] = ReadInt($"Товар позиции {i + 1}: ");
        quantities[i] = ReadInt("Количество: ");
    }
    using SqlConnection connection = new SqlConnection(connectionString);
    connection.Open();
    using SqlTransaction transaction = connection.BeginTransaction();
    int orderId = 0;
    try
    {
        using SqlCommand customerCommand = new SqlCommand(
            "SELECT COUNT(*) FROM customers WHERE id = @id;", connection, transaction);
        customerCommand.Parameters.AddWithValue("@id", customer);
        if (Convert.ToInt32(customerCommand.ExecuteScalar()) != 1)
            throw new InvalidOperationException("Клиент не найден.");

        decimal[] prices = new decimal[itemCount];
        for (int i = 0; i < itemCount; i++)
        {
            using SqlCommand product = new SqlCommand(
                "SELECT price FROM products WHERE id = @id;", connection, transaction);
            product.Parameters.AddWithValue("@id", products[i]);
            object? value = product.ExecuteScalar();
            if (value == null || value == DBNull.Value)
                throw new InvalidOperationException("Товар не найден.");
            prices[i] = Convert.ToDecimal(value);
        }

        using SqlCommand order = new SqlCommand(@"
INSERT INTO orders (customer_id, order_date, status)
VALUES (@customer_id, SYSDATETIME(), N'Новый');
SELECT CAST(SCOPE_IDENTITY() AS int);", connection, transaction);
        order.Parameters.AddWithValue("@customer_id", customer);
        orderId = Convert.ToInt32(order.ExecuteScalar());
        if (forceError)
            throw new InvalidOperationException("Тестовая ошибка после добавления заказа.");

        decimal total = 0;
        for (int i = 0; i < itemCount; i++)
        {
            using SqlCommand item = new SqlCommand(@"
INSERT INTO order_items (order_id, product_id, quantity, unit_price)
VALUES (@order_id, @product_id, @quantity, @unit_price);", connection, transaction);
            item.Parameters.AddWithValue("@order_id", orderId);
            item.Parameters.AddWithValue("@product_id", products[i]);
            item.Parameters.AddWithValue("@quantity", quantities[i]);
            item.Parameters.AddWithValue("@unit_price", prices[i]);
            if (item.ExecuteNonQuery() != 1)
                throw new InvalidOperationException("Позиция не добавлена.");
            total += prices[i] * quantities[i];
        }
        transaction.Commit();
        testOrders.Add(orderId);
        Console.WriteLine($"Заказ {orderId} создан. Сумма: {total:F2} руб.");
    }
    catch (Exception ex)
    {
        Rollback(transaction);
        Console.WriteLine($"Заказ не создан: {ex.Message}");
    }
    if (forceError && orderId > 0)
    {
        using SqlCommand check = new SqlCommand(
            "SELECT COUNT(*) FROM orders WHERE id = @id;", connection);
        check.Parameters.AddWithValue("@id", orderId);
        Console.WriteLine($"Осталось записей заказа {orderId}: {check.ExecuteScalar()}");
    }
}

void DeleteOrder(bool forceError)
{
    Console.WriteLine("Тестовые заказы текущего запуска: " + string.Join(", ", testOrders));
    int id = ReadInt("ID тестового заказа: ");
    if (!testOrders.Contains(id))
    {
        Console.WriteLine("Можно удалить только заказ, созданный этим запуском программы.");
        return;
    }
    using SqlConnection connection = new SqlConnection(connectionString);
    connection.Open();
    using SqlTransaction transaction = connection.BeginTransaction();
    try
    {
        using SqlCommand items = new SqlCommand(
            "DELETE FROM order_items WHERE order_id = @id;", connection, transaction);
        items.Parameters.AddWithValue("@id", id);
        int rows = items.ExecuteNonQuery();
        if (forceError)
            throw new InvalidOperationException("Тестовая ошибка после удаления позиций.");
        using SqlCommand order = new SqlCommand(
            "DELETE FROM orders WHERE id = @id;", connection, transaction);
        order.Parameters.AddWithValue("@id", id);
        if (order.ExecuteNonQuery() != 1)
            throw new InvalidOperationException("Заказ не найден.");
        transaction.Commit();
        testOrders.Remove(id);
        Console.WriteLine($"Заказ удалён вместе с позициями: {rows}.");
    }
    catch (Exception ex)
    {
        Rollback(transaction);
        Console.WriteLine($"Удаление отменено: {ex.Message}");
    }
    using SqlCommand check = new SqlCommand(@"
SELECT (SELECT COUNT(*) FROM orders WHERE id = @id),
       (SELECT COUNT(*) FROM order_items WHERE order_id = @id);", connection);
    check.Parameters.AddWithValue("@id", id);
    using SqlDataReader reader = check.ExecuteReader();
    if (reader.Read())
        Console.WriteLine($"Осталось заказов: {reader.GetInt32(0)}, позиций: {reader.GetInt32(1)}");
}

void Rollback(SqlTransaction transaction)
{
    try { transaction.Rollback(); }
    catch (Exception ex) { Console.WriteLine($"Ошибка отката: {ex.Message}"); }
}

void ShowPrices(int first, int second)
{
    using SqlConnection connection = new SqlConnection(connectionString);
    connection.Open();
    using SqlCommand command = new SqlCommand(
        "SELECT id, name, price FROM products WHERE id IN (@one, @two);", connection);
    command.Parameters.AddWithValue("@one", first);
    command.Parameters.AddWithValue("@two", second);
    using SqlDataReader reader = command.ExecuteReader();
    Console.WriteLine("Текущие цены:");
    while (reader.Read())
        Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)}: {reader.GetDecimal(2):F2}");
}

void Show(string sql)
{
    using SqlConnection connection = new SqlConnection(connectionString);
    connection.Open();
    using SqlCommand command = new SqlCommand(sql, connection);
    using SqlDataReader reader = command.ExecuteReader();
    if (!reader.HasRows) Console.WriteLine("Записи не найдены.");
    while (reader.Read())
    {
        for (int i = 0; i < reader.FieldCount; i++)
            Console.Write($"{reader.GetValue(i)} | ");
        Console.WriteLine();
    }
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
