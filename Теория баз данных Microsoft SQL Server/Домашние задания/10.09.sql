CREATE DATABASE CollegeDB;
GO
USE CollegeDB;
GO
CREATE SCHEMA college;
GO
CREATE TABLE college.students
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    age INT NOT NULL
);
INSERT INTO college.students (name, age)
VALUES (N'Иван', 18), (N'Мария', 17), (N'Алексей', 18),
       (N'Анна', 19), (N'Дмитрий', 17);
GO

SELECT * FROM college.students;

SELECT TOP 3 * FROM college.students ORDER BY id;

SELECT COUNT(*) AS student_count FROM college.students;

DECLARE @studentCount INT;
SELECT @studentCount = COUNT(*) FROM college.students;
SELECT @studentCount AS student_count;

SELECT * FROM college.students WHERE age > 17;

SELECT GETDATE() AS current_datetime;

SELECT * FROM sys.schemas;
