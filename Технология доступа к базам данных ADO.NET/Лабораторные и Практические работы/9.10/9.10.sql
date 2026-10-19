USE CollegeDB;
GO

SELECT * FROM college.groups;
SELECT * FROM college.students;
SELECT * FROM college.grades;

SELECT s.name, g.name AS group_name, s.average_grade,
       COUNT(gr.id) AS grade_count,
       AVG(CAST(gr.grade AS DECIMAL(4,2))) AS grades_average
FROM college.students s
JOIN college.groups g ON g.id = s.group_id
LEFT JOIN college.grades gr ON gr.student_id = s.id
GROUP BY s.id, s.name, g.name, s.average_grade;

;WITH StudentStatistics AS
(
    SELECT id, name, group_id, average_grade
    FROM college.students
)
SELECT name, average_grade,
       RANK() OVER (ORDER BY average_grade DESC) AS rating
FROM StudentStatistics
ORDER BY rating, id;

;WITH StudentStatistics AS
(
    SELECT id, name, group_id, average_grade
    FROM college.students
)
SELECT s.name AS student_name, g.name AS group_name, s.average_grade,
       RANK() OVER (
           PARTITION BY s.group_id ORDER BY s.average_grade DESC
       ) AS group_rating
FROM StudentStatistics s
JOIN college.groups g ON g.id = s.group_id
ORDER BY s.group_id, group_rating, s.id;
GO

CREATE OR ALTER VIEW college.StudentRating
AS
SELECT s.id, s.name, g.name AS group_name, s.average_grade,
       RANK() OVER (
           PARTITION BY s.group_id ORDER BY s.average_grade DESC
       ) AS group_rating
FROM college.students s
JOIN college.groups g ON g.id = s.group_id;
GO

SELECT * FROM college.StudentRating;
SELECT * FROM college.StudentRating WHERE group_name = N'ИС-31';
GO

CREATE OR ALTER PROCEDURE college.GetStudentsByMinGrade
    @min_grade DECIMAL(4,2)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id, name, age, average_grade, group_id
    FROM college.students
    WHERE average_grade >= @min_grade
    ORDER BY average_grade DESC, id;
END;
GO

EXEC college.GetStudentsByMinGrade @min_grade = 4.5;
EXEC college.GetStudentsByMinGrade @min_grade = 4.8;
GO

CREATE OR ALTER FUNCTION college.GetStudentStatus
(
    @average_grade DECIMAL(4,2)
)
RETURNS NVARCHAR(100)
AS
BEGIN
    RETURN CASE
        WHEN @average_grade >= 4.5 THEN N'Отличник'
        WHEN @average_grade >= 3.5 THEN N'Хорошист'
        ELSE N'Требуется дополнительная подготовка'
    END;
END;
GO

SELECT name, average_grade,
       college.GetStudentStatus(average_grade) AS status
FROM college.students;
GO

CREATE OR ALTER FUNCTION college.GetStudentsByGroup
(
    @group_id INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT id, name, age, average_grade
    FROM college.students
    WHERE group_id = @group_id
);
GO

SELECT * FROM college.GetStudentsByGroup(1);
SELECT * FROM college.GetStudentsByGroup(1) WHERE average_grade >= 4.5;
GO

IF OBJECT_ID(N'college.student_changes', N'U') IS NULL
BEGIN
    CREATE TABLE college.student_changes
    (
        id INT IDENTITY(1,1) PRIMARY KEY,
        student_id INT NOT NULL,
        old_average_grade DECIMAL(4,2),
        new_average_grade DECIMAL(4,2),
        change_date DATETIME2 NOT NULL
    );
END;
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
    WHERE i.average_grade <> d.average_grade;
END;
GO

UPDATE college.students
SET average_grade = average_grade + 0.1
WHERE id = 1;

SELECT * FROM college.student_changes ORDER BY id;

UPDATE college.students
SET average_grade = average_grade + 0.1
WHERE group_id = 1;

SELECT * FROM college.student_changes ORDER BY id;

SELECT r.name AS student_name, r.group_name, r.average_grade,
       college.GetStudentStatus(r.average_grade) AS status,
       r.group_rating, COUNT(gr.id) AS grade_count
FROM college.StudentRating r
LEFT JOIN college.grades gr ON gr.student_id = r.id
GROUP BY r.id, r.name, r.group_name, r.average_grade, r.group_rating
ORDER BY r.group_name, r.group_rating, r.id;
