USE CollegeDB;
GO

IF OBJECT_ID(N'college.groups', N'U') IS NULL
BEGIN
    CREATE TABLE college.groups (
        id   INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(50) NOT NULL
    );
END
GO

IF COL_LENGTH(N'college.students', N'group_id') IS NULL
BEGIN
    ALTER TABLE college.students ADD group_id INT NULL;
END
GO

INSERT INTO college.groups (name)
SELECT v.name
FROM (VALUES (N'ИС-21'), (N'ПР-21'), (N'СА-21')) AS v(name)
WHERE NOT EXISTS (SELECT 1 FROM college.groups g WHERE g.name = v.name);
GO

INSERT INTO college.students (name, age, average_grade, group_id)
SELECT v.name, v.age, v.average_grade, g.id
FROM (VALUES
        (N'Иван',    18, 4.7, N'ИС-21'),
        (N'Мария',   17, 4.2, N'ИС-21'),
        (N'Алексей', 18, 3.6, N'ИС-21'),
        (N'Анна',    19, 4.9, N'ПР-21'),
        (N'Дмитрий', 17, 2.8, N'ПР-21'),
        (N'Елена',   18, 4.5, N'СА-21'),
        (N'Максим',  19, 3.9, N'СА-21')
     ) AS v(name, age, average_grade, group_name)
JOIN college.groups AS g ON g.name = v.group_name
WHERE NOT EXISTS (SELECT 1 FROM college.students s WHERE s.name = v.name);
GO

UPDATE s
SET s.group_id = g.id
FROM college.students AS s
JOIN (VALUES
        (N'Иван',    N'ИС-21'),
        (N'Мария',   N'ИС-21'),
        (N'Алексей', N'ИС-21'),
        (N'Анна',    N'ПР-21'),
        (N'Дмитрий', N'ПР-21'),
        (N'Елена',   N'СА-21'),
        (N'Максим',  N'СА-21')
     ) AS v(name, group_name) ON v.name = s.name
JOIN college.groups AS g ON g.name = v.group_name
WHERE s.group_id IS NULL;
GO

SELECT * FROM college.groups;
SELECT * FROM college.students;
GO

SELECT name, average_grade
FROM college.students
WHERE average_grade > (
    SELECT AVG(average_grade)
    FROM college.students
);
GO

SELECT name, age
FROM college.students
WHERE age > (
    SELECT AVG(CAST(age AS DECIMAL(5,2)))
    FROM college.students
);
GO

SELECT name, average_grade
FROM college.students
WHERE average_grade = (
    SELECT MAX(average_grade)
    FROM college.students
);
GO

SELECT name, group_id
FROM college.students
WHERE group_id IN (
    SELECT id
    FROM college.groups
    WHERE name LIKE N'%ИС%'
);
GO

SELECT name, group_id
FROM college.students
WHERE group_id IN (
    SELECT id
    FROM college.groups
    WHERE name LIKE N'%21'
);
GO

SELECT name
FROM college.groups
WHERE id IN (
    SELECT group_id
    FROM college.students
    WHERE average_grade > 4.5
);
GO

SELECT g.name
FROM college.groups AS g
WHERE EXISTS (
    SELECT 1
    FROM college.students AS s
    WHERE s.group_id = g.id
      AND s.average_grade > 4.5
);
GO

SELECT g.name
FROM college.groups AS g
WHERE EXISTS (
    SELECT 1
    FROM college.students AS s
    WHERE s.group_id = g.id
      AND s.age < 18
);
GO

SELECT g.name
FROM college.groups AS g
WHERE NOT EXISTS (
    SELECT 1
    FROM college.students AS s
    WHERE s.group_id = g.id
      AND s.average_grade < 3.0
);
GO

SELECT s.name, s.average_grade, s.group_id
FROM college.students AS s
WHERE s.average_grade > (
    SELECT AVG(s2.average_grade)
    FROM college.students AS s2
    WHERE s2.group_id = s.group_id
);
GO

SELECT s.name, s.age, s.group_id
FROM college.students AS s
WHERE s.age > (
    SELECT AVG(CAST(s2.age AS DECIMAL(5,2)))
    FROM college.students AS s2
    WHERE s2.group_id = s.group_id
);
GO

SELECT s.name, s.average_grade, s.group_id
FROM college.students AS s
WHERE s.average_grade < (
    SELECT AVG(s2.average_grade)
    FROM college.students AS s2
    WHERE s2.group_id = s.group_id
);
GO

SELECT name
FROM college.groups
WHERE id IN (
    SELECT group_id
    FROM college.students
    WHERE average_grade >= 4.5
);
GO

SELECT g.name
FROM college.groups AS g
WHERE EXISTS (
    SELECT 1
    FROM college.students AS s
    WHERE s.group_id = g.id
      AND s.average_grade >= 4.5
);
GO

(
    SELECT name FROM college.groups
    WHERE id IN (SELECT group_id FROM college.students WHERE average_grade >= 4.5)
    EXCEPT
    SELECT g.name FROM college.groups AS g
    WHERE EXISTS (SELECT 1 FROM college.students AS s
                  WHERE s.group_id = g.id AND s.average_grade >= 4.5)
)
UNION ALL
(
    SELECT g.name FROM college.groups AS g
    WHERE EXISTS (SELECT 1 FROM college.students AS s
                  WHERE s.group_id = g.id AND s.average_grade >= 4.5)
    EXCEPT
    SELECT name FROM college.groups
    WHERE id IN (SELECT group_id FROM college.students WHERE average_grade >= 4.5)
);

GO

SELECT s.name, s.average_grade
FROM college.students AS s
WHERE s.group_id IN (
    SELECT id
    FROM college.groups
    WHERE name = N'ПР-21'
)
AND s.average_grade > (
    SELECT AVG(s2.average_grade)
    FROM college.students AS s2
    WHERE s2.group_id = s.group_id
);
GO

SELECT s.name, s.age, s.average_grade, s.group_id
FROM college.students AS s
WHERE s.age < (
    SELECT AVG(CAST(s2.age AS DECIMAL(5,2)))
    FROM college.students AS s2
    WHERE s2.group_id = s.group_id
)
AND s.average_grade > 4.0;
GO

SELECT
    s.name          AS [Студент],
    s.average_grade AS [Средний балл],
    g.name          AS [Группа],
    (
        SELECT CAST(AVG(s2.average_grade) AS DECIMAL(3,2))
        FROM college.students AS s2
        WHERE s2.group_id = s.group_id
    )               AS [Средний балл группы]
FROM college.students AS s
JOIN college.groups   AS g ON g.id = s.group_id
WHERE s.average_grade > (
    SELECT AVG(s2.average_grade)
    FROM college.students AS s2
    WHERE s2.group_id = s.group_id
);
GO
