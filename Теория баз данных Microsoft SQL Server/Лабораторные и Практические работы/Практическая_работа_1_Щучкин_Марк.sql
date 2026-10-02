USE CollegeDB;
GO
SET NOCOUNT ON;
GO

SELECT * FROM college.students;
GO

IF NOT EXISTS (SELECT 1 FROM college.students)
BEGIN
    INSERT INTO college.students (name, age, average_grade)
    VALUES
        (N'Иван',    18, 4.7),
        (N'Мария',   17, 4.2),
        (N'Алексей', 18, 3.6),
        (N'Анна',    19, 4.9),
        (N'Дмитрий', 17, 2.8);
END
GO

DECLARE @age INT = 18;
SELECT @age AS age;
GO

DECLARE @age INT = 18;
SET @age = 19;
SELECT @age AS age;
GO

DECLARE @name         NVARCHAR(100) = N'Иван';
DECLARE @age          INT           = 18;
DECLARE @averageGrade DECIMAL(3,2)  = 4.70;

SELECT @name         AS name,
       @age          AS age,
       @averageGrade AS average_grade;
GO

DECLARE @studentName NVARCHAR(100);

SELECT @studentName = name
FROM college.students
WHERE id = 1;

SELECT @studentName AS student_name;
GO

DECLARE @studentName  NVARCHAR(100);
DECLARE @studentAge   INT;
DECLARE @studentGrade DECIMAL(3,2);

SELECT @studentName  = name,
       @studentAge   = age,
       @studentGrade = average_grade
FROM college.students
WHERE id = 1;

SELECT @studentName  AS name,
       @studentAge   AS age,
       @studentGrade AS average_grade;
GO

DECLARE @studentCount INT;

SELECT @studentCount = COUNT(*)
FROM college.students;

SELECT @studentCount AS student_count;
GO

DECLARE @age INT = 18;

SELECT @age     AS current_age,
       @age + 5 AS future_age;
GO

DECLARE @grade1 INT = 5;
DECLARE @grade2 INT = 4;
DECLARE @grade3 INT = 4;

SELECT (@grade1 + @grade2 + @grade3) / 3 AS average_int;
GO

DECLARE @grade1 DECIMAL(4,2) = 5;
DECLARE @grade2 DECIMAL(4,2) = 4;
DECLARE @grade3 DECIMAL(4,2) = 4;

SELECT (@grade1 + @grade2 + @grade3) / 3 AS average_decimal;
GO

DECLARE @price      DECIMAL(10,2) = 1500;
DECLARE @discount   INT           = 10;
DECLARE @finalPrice DECIMAL(10,2);

SET @finalPrice = @price - (@price * @discount / 100);

PRINT N'Цена:          ' + CAST(@price AS NVARCHAR(20));
PRINT N'Скидка:        ' + CAST(@discount AS NVARCHAR(10)) + N'%';
PRINT N'Итоговая цена: ' + CAST(@finalPrice AS NVARCHAR(20));
GO

DECLARE @age INT = 18;

IF @age >= 18
    PRINT N'Совершеннолетний';
ELSE
    PRINT N'Несовершеннолетний';
GO

DECLARE @grade DECIMAL(3,2) = 3.5;

IF @grade >= 3
    PRINT N'Студент успевает';
ELSE
    PRINT N'Есть задолженность';
GO

DECLARE @price DECIMAL(10,2) = 6000;

IF @price >= 5000
    PRINT N'Большая покупка';
ELSE
    PRINT N'Обычная покупка';
GO

DECLARE @age INT = 18;

IF @age < 18
    PRINT N'Несовершеннолетний';
ELSE IF @age BETWEEN 18 AND 19
    PRINT N'18–19 лет';
ELSE
    PRINT N'20+';
GO

DECLARE @averageGrade DECIMAL(3,2) = 4.70;

IF @averageGrade >= 4.5
    PRINT N'Отличник';
ELSE IF @averageGrade >= 3.5
    PRINT N'Хорошист';
ELSE IF @averageGrade >= 3.0
    PRINT N'Успевает';
ELSE
    PRINT N'Есть задолженности';
GO

SELECT
    name,
    age,
    CASE
        WHEN age < 18             THEN N'Младше 18'
        WHEN age BETWEEN 18 AND 19 THEN N'18–19 лет'
        ELSE                           N'20+'
    END AS age_group
FROM college.students;
GO

SELECT
    name,
    average_grade,
    CASE
        WHEN average_grade >= 4.5 THEN N'Отличник'
        WHEN average_grade >= 3.5 THEN N'Хорошист'
        WHEN average_grade >= 3.0 THEN N'Успевает'
        ELSE                           N'Есть задолженности'
    END AS result
FROM college.students;
GO

SELECT
    name,
    age,
    CASE
        WHEN age >= 18 THEN N'Совершеннолетний'
        ELSE                N'Несовершеннолетний'
    END AS age_status
FROM college.students;
GO

DECLARE @averageGroupGrade DECIMAL(3,2);

SELECT @averageGroupGrade = AVG(average_grade)
FROM college.students;

SELECT @averageGroupGrade AS average_group_grade;
GO

DECLARE @averageGroupGrade DECIMAL(3,2);

SELECT @averageGroupGrade = AVG(average_grade)
FROM college.students;

IF @averageGroupGrade >= 4.5
    PRINT N'Группа показывает отличный результат';
ELSE IF @averageGroupGrade >= 3.5
    PRINT N'Группа показывает хороший результат';
ELSE IF @averageGroupGrade >= 3.0
    PRINT N'Группа успевает';
ELSE
    PRINT N'Группе необходимо улучшить результаты';
GO

DECLARE @studentId    INT = 1;
DECLARE @studentName  NVARCHAR(100);
DECLARE @studentAge   INT;
DECLARE @studentGrade DECIMAL(3,2);
DECLARE @ageGroup     NVARCHAR(30);
DECLARE @result       NVARCHAR(30);

SELECT @studentName  = name,
       @studentAge   = age,
       @studentGrade = average_grade
FROM college.students
WHERE id = @studentId;

IF @studentName IS NULL
BEGIN
    PRINT N'Студент с id = ' + CAST(@studentId AS NVARCHAR(10)) + N' не найден';
END
ELSE
BEGIN

    SET @ageGroup = CASE
                        WHEN @studentAge < 18              THEN N'Младше 18'
                        WHEN @studentAge BETWEEN 18 AND 19 THEN N'18–19 лет'
                        ELSE                                    N'20+'
                    END;

    SET @result = CASE
                      WHEN @studentGrade >= 4.5 THEN N'Отличник'
                      WHEN @studentGrade >= 3.5 THEN N'Хорошист'
                      WHEN @studentGrade >= 3.0 THEN N'Успевает'
                      ELSE                           N'Есть задолженности'
                  END;

    PRINT N'-----------------------------------';
    PRINT N'Информация о студенте';
    PRINT N'-----------------------------------';
    PRINT N'';
    PRINT N'Имя: '              + @studentName;
    PRINT N'Возраст: '          + CAST(@studentAge   AS NVARCHAR(10));
    PRINT N'Возрастная группа: ' + @ageGroup;
    PRINT N'Средний балл: '     + CAST(@studentGrade AS NVARCHAR(10));
    PRINT N'Результат: '        + @result;
END
GO
