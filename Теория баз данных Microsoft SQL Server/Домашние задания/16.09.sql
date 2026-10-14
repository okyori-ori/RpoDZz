USE CollegeDB;
GO
ALTER TABLE college.students ADD average_grade DECIMAL(4,2);

UPDATE college.students
SET average_grade = CASE id
    WHEN 1 THEN 4.7
    WHEN 2 THEN 4.2
    WHEN 3 THEN 3.6
    WHEN 4 THEN 4.9
    WHEN 5 THEN 2.8
END
WHERE id BETWEEN 1 AND 5;
GO

DECLARE @studentCount INT;
SELECT @studentCount = COUNT(*) FROM college.students;
SELECT @studentCount AS student_count;
GO

DECLARE @studentName NVARCHAR(100);
SELECT @studentName = name FROM college.students WHERE id = 1;
SELECT @studentName AS student_name;
GO

DECLARE @age INT;
SELECT @age = age FROM college.students WHERE id = 1;
IF @age >= 18
    PRINT N'Совершеннолетний';
ELSE
    PRINT N'Несовершеннолетний';
GO

SELECT *,
    CASE WHEN age >= 18 THEN N'Совершеннолетний'
         ELSE N'Несовершеннолетний' END AS age_status
FROM college.students;

SELECT name, average_grade,
    CASE WHEN average_grade >= 4.5 THEN N'Отличник'
         WHEN average_grade >= 3.5 THEN N'Хорошист'
         WHEN average_grade >= 3.0 THEN N'Успевает'
         ELSE N'Есть задолженности' END AS result
FROM college.students;

DECLARE @price DECIMAL(10,2) = 1500;
DECLARE @discount DECIMAL(5,2);
IF @price >= 1000
    SET @discount = 10;
ELSE
    SET @discount = 5;
SELECT @price AS price, @discount AS discount,
       @price - @price * @discount / 100 AS final_price;
GO

SELECT name, age,
    CASE WHEN age < 18 THEN N'Младше 18'
         WHEN age <= 19 THEN N'18-19 лет'
         ELSE N'20+' END AS age_group
FROM college.students;

DECLARE @studentId INT = 1;
DECLARE @studentName NVARCHAR(100);
DECLARE @age INT;
DECLARE @averageGrade DECIMAL(4,2);

SELECT @studentName = name, @age = age,
       @averageGrade = average_grade
FROM college.students WHERE id = @studentId;

SELECT @studentName AS name, @age AS age,
       @averageGrade AS average_grade,
    CASE WHEN @age < 18 THEN N'Младше 18'
         WHEN @age <= 19 THEN N'18-19 лет'
         ELSE N'20+' END AS age_group,
    CASE WHEN @averageGrade >= 4.5 THEN N'Отличник'
         WHEN @averageGrade >= 3.5 THEN N'Хорошист'
         WHEN @averageGrade >= 3.0 THEN N'Успевает'
         ELSE N'Есть задолженности' END AS result;
