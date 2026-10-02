USE master;
GO

IF DB_ID('CookingDB') IS NULL
BEGIN
    CREATE DATABASE CookingDB;
END;
GO

USE CookingDB;
GO

IF SCHEMA_ID('kitchen') IS NULL
    EXEC('CREATE SCHEMA kitchen');
GO

IF OBJECT_ID('kitchen.categories', 'U') IS NULL
BEGIN
    CREATE TABLE kitchen.categories
    (
        id INT PRIMARY KEY,
        name NVARCHAR(100) NOT NULL
    );
END;
GO

IF OBJECT_ID('kitchen.dishes', 'U') IS NULL
BEGIN
    CREATE TABLE kitchen.dishes
    (
        id INT PRIMARY KEY,
        name NVARCHAR(100) NOT NULL,
        category_id INT NOT NULL,
        price DECIMAL(10, 2) NOT NULL,

        CONSTRAINT FK_dishes_categories
            FOREIGN KEY (category_id)
            REFERENCES kitchen.categories(id)
    );
END;
GO

IF OBJECT_ID('kitchen.orders', 'U') IS NULL
BEGIN
    CREATE TABLE kitchen.orders
    (
        id INT PRIMARY KEY,
        customer_name NVARCHAR(100) NOT NULL,
        order_date DATE NOT NULL
    );
END;
GO

IF OBJECT_ID('kitchen.order_items', 'U') IS NULL
BEGIN
    CREATE TABLE kitchen.order_items
    (
        id INT PRIMARY KEY,
        order_id INT NOT NULL,
        dish_id INT NOT NULL,
        quantity INT NOT NULL,

        CONSTRAINT FK_order_items_orders
            FOREIGN KEY (order_id)
            REFERENCES kitchen.orders(id),

        CONSTRAINT FK_order_items_dishes
            FOREIGN KEY (dish_id)
            REFERENCES kitchen.dishes(id)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM kitchen.categories)
BEGIN
    INSERT INTO kitchen.categories (id, name)
    VALUES
        (1, N'Пицца'),
        (2, N'Супы'),
        (3, N'Основные блюда'),
        (4, N'Десерты');
END;
GO

IF NOT EXISTS (SELECT 1 FROM kitchen.dishes)
BEGIN
    INSERT INTO kitchen.dishes
        (id, name, category_id, price)
    VALUES
        (1, N'Маргарита', 1, 550),
        (2, N'Пепперони', 1, 650),
        (3, N'Том Ям', 2, 480),
        (4, N'Крем-суп грибной', 2, 350),
        (5, N'Паста Карбонара', 3, 590),
        (6, N'Стейк с овощами', 3, 950),
        (7, N'Чизкейк', 4, 320),
        (8, N'Тирамису', 4, 380);
END;
GO

IF NOT EXISTS (SELECT 1 FROM kitchen.orders)
BEGIN
    INSERT INTO kitchen.orders
        (id, customer_name, order_date)
    VALUES
        (1, N'Анна', '2026-09-01'),
        (2, N'Иван', '2026-09-02'),
        (3, N'Мария', '2026-09-03'),
        (4, N'Алексей', '2026-09-04'),
        (5, N'Ольга', '2026-09-05');
END;
GO

IF NOT EXISTS (SELECT 1 FROM kitchen.order_items)
BEGIN
    INSERT INTO kitchen.order_items
        (id, order_id, dish_id, quantity)
    VALUES
        (1, 1, 1, 2),
        (2, 1, 7, 1),
        (3, 2, 2, 1),
        (4, 2, 5, 1),
        (5, 3, 3, 2),
        (6, 3, 8, 1),
        (7, 4, 6, 1),
        (8, 4, 4, 1),
        (9, 5, 1, 1),
        (10, 5, 8, 2);
END;
GO

CREATE OR ALTER VIEW kitchen.menu
AS
SELECT
    d.name AS dish_name,
    c.name AS category_name,
    d.price
FROM kitchen.dishes d
JOIN kitchen.categories c
    ON d.category_id = c.id;
GO

SELECT *
FROM kitchen.menu;
GO

CREATE OR ALTER VIEW kitchen.order_details
AS
SELECT
    o.id AS order_id,
    o.customer_name,
    o.order_date,
    d.name AS dish_name,
    oi.quantity,
    d.price
FROM kitchen.orders o
JOIN kitchen.order_items oi
    ON o.id = oi.order_id
JOIN kitchen.dishes d
    ON oi.dish_id = d.id;
GO

SELECT *
FROM kitchen.order_details;
GO

CREATE OR ALTER VIEW kitchen.order_totals
AS
SELECT
    o.id AS order_id,
    o.customer_name,
    o.order_date,
    SUM(d.price * oi.quantity) AS total_price
FROM kitchen.orders o
JOIN kitchen.order_items oi
    ON o.id = oi.order_id
JOIN kitchen.dishes d
    ON oi.dish_id = d.id
GROUP BY
    o.id,
    o.customer_name,
    o.order_date;
GO

SELECT *
FROM kitchen.order_totals;
GO

SELECT *
FROM kitchen.menu
WHERE price < 500;
GO

SELECT *
FROM kitchen.order_totals
WHERE total_price > 1000;
GO

SELECT TOP 1
    *
FROM kitchen.order_totals
ORDER BY total_price DESC;
GO

ALTER VIEW kitchen.menu
AS
SELECT
    d.name AS dish_name,
    c.name AS category_name,
    d.price,
    CASE
        WHEN d.price < 500
            THEN N'Обычное'
        WHEN d.price <= 700
            THEN N'Средняя цена'
        ELSE N'Дорогое'
    END AS price_category
FROM kitchen.dishes d
JOIN kitchen.categories c
    ON d.category_id = c.id;
GO

SELECT *
FROM kitchen.menu;
GO

SELECT
    customer_name,
    total_price,
    RANK() OVER
    (
        ORDER BY total_price DESC
    ) AS rating
FROM kitchen.order_totals
ORDER BY rating, customer_name;
GO

SELECT *
FROM kitchen.menu;
GO

SELECT *
FROM kitchen.order_details;
GO

SELECT *
FROM kitchen.order_totals;
GO
