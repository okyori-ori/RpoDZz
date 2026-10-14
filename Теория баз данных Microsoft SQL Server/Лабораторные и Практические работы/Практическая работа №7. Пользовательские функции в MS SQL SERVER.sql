USE CollegeDB;
GO

SELECT * FROM college.groups;
SELECT * FROM college.students;
SELECT * FROM college.grades;
GO

CREATE OR ALTER FUNCTION college.GetStudentStatus
(
    @average_grade DECIMAL(4,2)
)
RETURNS NVARCHAR(100)
AS
BEGIN
    RETURN CASE
        WHEN @average_grade BETWEEN 4.5 AND 5.0 THEN N'Отличник'
        WHEN @average_grade >= 3.5 AND @average_grade < 4.5 THEN N'Хорошист'
        WHEN @average_grade < 3.5 THEN N'Требуется дополнительная подготовка'
        ELSE NULL
    END;
END;
GO

SELECT name, average_grade,
       college.GetStudentStatus(average_grade) AS status
FROM college.students;

SELECT college.GetStudentStatus(5.0) AS status;
SELECT college.GetStudentStatus(4.2) AS status;
SELECT college.GetStudentStatus(3.1) AS status;
SELECT college.GetStudentStatus(4.5) AS status;
SELECT college.GetStudentStatus(3.5) AS status;
GO

CREATE OR ALTER FUNCTION college.CalculateDiscount
(
    @price DECIMAL(12,2),
    @discount DECIMAL(5,2)
)
RETURNS DECIMAL(12,2)
AS
BEGIN
    RETURN @price - @price * @discount / 100;
END;
GO

SELECT college.CalculateDiscount(10000, 10) AS final_price;
SELECT college.CalculateDiscount(25000, 15) AS final_price;

SELECT name, average_grade,
       college.CalculateDiscount(10000, 10) AS discounted_price
FROM college.students;

SELECT s.name, g.name AS group_name, s.average_grade,
       college.GetStudentStatus(s.average_grade) AS status
FROM college.students s
JOIN college.groups g ON g.id = s.group_id;
GO

CREATE OR ALTER FUNCTION college.GetStudentsByGroup
(
    @group_id INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT id, name, age, average_grade, group_id
    FROM college.students
    WHERE group_id = @group_id
);
GO

SELECT * FROM college.GetStudentsByGroup(1);
SELECT * FROM college.GetStudentsByGroup(2);

SELECT s.name, g.name AS group_name, s.average_grade
FROM college.GetStudentsByGroup(1) s
JOIN college.groups g ON g.id = s.group_id;
GO

CREATE OR ALTER FUNCTION college.GetAgeCategory
(
    @age INT
)
RETURNS NVARCHAR(50)
AS
BEGIN
    RETURN CASE
        WHEN @age < 18 THEN N'Несовершеннолетний'
        WHEN @age BETWEEN 18 AND 20 THEN N'Студент'
        WHEN @age > 20 THEN N'Взрослый'
        ELSE NULL
    END;
END;
GO

SELECT college.GetAgeCategory(17) AS age_category;
SELECT college.GetAgeCategory(19) AS age_category;
SELECT college.GetAgeCategory(25) AS age_category;
SELECT college.GetAgeCategory(18) AS age_category;
SELECT college.GetAgeCategory(20) AS age_category;

SELECT name, age, college.GetAgeCategory(age) AS age_category
FROM college.students;

SELECT s.name, g.name AS group_name, s.age, s.average_grade,
       college.GetStudentStatus(s.average_grade) AS status,
       college.GetAgeCategory(s.age) AS age_category
FROM college.students s
JOIN college.groups g ON g.id = s.group_id;

SELECT name, average_grade
FROM college.students
WHERE college.GetStudentStatus(average_grade) = N'Отличник';

SELECT question, answer
FROM (VALUES
    (1, N'Что такое пользовательская функция?',
        N'Объект базы данных, который принимает параметры и возвращает результат.'),
    (2, N'Чем scalar-функция отличается от table-valued function?',
        N'Scalar-функция возвращает одно значение, табличная функция возвращает таблицу.'),
    (3, N'Что возвращает scalar-функция?',
        N'Одно значение указанного в RETURNS типа.'),
    (4, N'Что возвращает inline table-valued function?',
        N'Таблицу, полученную одним запросом SELECT.'),
    (5, N'Где можно использовать пользовательскую функцию?',
        N'Скалярную функцию можно использовать в SELECT и WHERE, табличную - в FROM и JOIN.'),
    (6, N'Чем функция отличается от хранимой процедуры?',
        N'Функция используется в запросе и возвращает результат. Процедура вызывается через EXEC и может изменять данные.'),
    (7, N'Чем функция отличается от VIEW?',
        N'Функция может принимать параметры. Обычное представление параметров не принимает.'),
    (8, N'Зачем функции передаются параметры?',
        N'Чтобы получать результат для разных исходных значений.'),
    (9, N'Почему функция должна возвращать определённый тип данных?',
        N'Чтобы SQL Server знал тип результата и мог использовать его в выражениях и запросах.'),
    (10, N'Когда функция упрощает SQL-запрос?',
        N'Когда одинаковое вычисление или выборка повторяются в нескольких запросах.')
) t(number, question, answer)
ORDER BY number;
