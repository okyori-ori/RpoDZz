USE CollegeDB;
GO

SELECT * FROM college.students;

CREATE TABLE college.student_changes
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL,
    old_average_grade DECIMAL(4,2),
    new_average_grade DECIMAL(4,2),
    change_date DATETIME2 NOT NULL
);
GO

CREATE OR ALTER TRIGGER college.trg_Students_AverageGradeAudit
ON college.students
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO college.student_changes
        (student_id, old_average_grade, new_average_grade, change_date)
    SELECT i.id, d.average_grade, i.average_grade, SYSDATETIME()
    FROM inserted i
    JOIN deleted d ON d.id = i.id
    WHERE i.average_grade <> d.average_grade
       OR (i.average_grade IS NULL AND d.average_grade IS NOT NULL)
       OR (i.average_grade IS NOT NULL AND d.average_grade IS NULL);
END;
GO

UPDATE college.students
SET average_grade = 4.8
WHERE id = 1;

SELECT * FROM college.student_changes ORDER BY id;

UPDATE college.students
SET average_grade = average_grade + 0.1
WHERE group_id = 1;

SELECT * FROM college.student_changes ORDER BY id;
GO

CREATE OR ALTER TRIGGER college.trg_Students_ValidateGrade
ON college.students
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted
        WHERE average_grade < 0 OR average_grade > 5
    )
    BEGIN
        THROW 50001, N'Средний балл должен находиться в диапазоне от 0 до 5', 1;
    END;
END;
GO

UPDATE college.students
SET average_grade = 4.5
WHERE id = 2;

SELECT * FROM college.students WHERE id = 2;
GO

BEGIN TRY
    UPDATE college.students
    SET average_grade = 6
    WHERE id = 2;
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS error_number,
           ERROR_MESSAGE() AS error_message;
END CATCH;

SELECT * FROM college.students WHERE id = 2;
GO

BEGIN TRY
    UPDATE college.students
    SET average_grade = -1
    WHERE id = 2;
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS error_number,
           ERROR_MESSAGE() AS error_message;
END CATCH;

SELECT * FROM college.students WHERE id = 2;
GO

SELECT trigger_name, event_name, used_data
FROM (VALUES
    (N'trg_Students_AverageGradeAudit', N'UPDATE', N'deleted, inserted'),
    (N'trg_Students_ValidateGrade', N'INSERT, UPDATE', N'inserted')
) t(trigger_name, event_name, used_data);

SELECT question, answer
FROM (VALUES
    (1, N'Что находилось в deleted?',
        N'Старые значения строк до изменения.'),
    (2, N'Что находилось в inserted?',
        N'Новые значения строк после изменения.'),
    (3, N'Почему при UPDATE нужны обе таблицы?',
        N'Чтобы сопоставить старую и новую строку по id и сравнить значения.'),
    (4, N'Почему нужно обрабатывать несколько строк?',
        N'Один UPDATE может изменить много студентов. Триггер запускается один раз для всей команды.')
) t(number, question, answer)
ORDER BY number;
GO

ALTER TABLE college.student_changes
ADD old_age INT, new_age INT;
GO

ALTER TRIGGER college.trg_Students_AverageGradeAudit
ON college.students
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO college.student_changes
        (student_id, old_average_grade, new_average_grade,
         change_date, old_age, new_age)
    SELECT i.id, d.average_grade, i.average_grade,
           SYSDATETIME(), d.age, i.age
    FROM inserted i
    JOIN deleted d ON d.id = i.id
    WHERE i.average_grade <> d.average_grade
       OR (i.average_grade IS NULL AND d.average_grade IS NOT NULL)
       OR (i.average_grade IS NOT NULL AND d.average_grade IS NULL)
       OR i.age <> d.age
       OR (i.age IS NULL AND d.age IS NOT NULL)
       OR (i.age IS NOT NULL AND d.age IS NULL);
END;
GO

UPDATE college.students
SET age = age + 1,
    average_grade = average_grade + 0.1
WHERE id = 3;

SELECT TOP 1 *
FROM college.student_changes
WHERE student_id = 3
ORDER BY id DESC;

SELECT question, answer
FROM (VALUES
    (1, N'Что такое DML-триггер?',
        N'Объект базы, автоматически выполняющийся при INSERT, UPDATE или DELETE.'),
    (2, N'Когда срабатывает триггер?',
        N'При событии, которое указано в его определении.'),
    (3, N'Чем AFTER отличается от INSTEAD OF?',
        N'AFTER выполняется после операции и проверки ограничений, INSTEAD OF выполняет свою логику вместо операции.'),
    (4, N'Что такое inserted?',
        N'Логическая таблица новых строк внутри триггера.'),
    (5, N'Что такое deleted?',
        N'Логическая таблица старых строк внутри триггера.'),
    (6, N'Что находится в inserted при UPDATE?',
        N'Новые версии изменённых строк.'),
    (7, N'Что находится в deleted при UPDATE?',
        N'Старые версии изменённых строк.'),
    (8, N'Может ли inserted содержать несколько строк?',
        N'Да, одна команда может изменить или добавить сразу несколько строк.'),
    (9, N'Почему нельзя рассчитывать только на одну строку?',
        N'Часть изменений будет пропущена, если команда затронет несколько строк.'),
    (10, N'Для чего используются триггеры?',
        N'Для аудита изменений и проверки сложных правил работы с данными.'),
    (11, N'Чем триггер отличается от хранимой процедуры?',
        N'Триггер запускается автоматически по событию, процедура вызывается явно через EXEC.'),
    (12, N'Когда лучше использовать CHECK или другие ограничения?',
        N'Для простых правил данных: диапазона значений, обязательности, уникальности или связи между таблицами.')
) t(number, question, answer)
ORDER BY number;
