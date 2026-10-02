USE CollegeDB;
GO

IF SCHEMA_ID('college') IS NULL
    EXEC('CREATE SCHEMA college');
GO

IF OBJECT_ID('college.groups', 'U') IS NULL
BEGIN
    CREATE TABLE college.groups
    (
        id INT PRIMARY KEY,
        name NVARCHAR(50) NOT NULL
    );
END;
GO

IF OBJECT_ID('college.students', 'U') IS NULL
BEGIN
    CREATE TABLE college.students
    (
        id INT PRIMARY KEY,
        name NVARCHAR(100) NOT NULL,
        group_id INT NOT NULL,
        CONSTRAINT FK_students_groups
            FOREIGN KEY (group_id)
            REFERENCES college.groups(id)
    );
END;
GO

IF OBJECT_ID('college.grades', 'U') IS NULL
BEGIN
    CREATE TABLE college.grades
    (
        id INT PRIMARY KEY,
        student_id INT NOT NULL,
        subject NVARCHAR(100) NOT NULL,
        grade DECIMAL(4,2) NOT NULL,
        grade_date DATE NOT NULL,
        CONSTRAINT FK_grades_students
            FOREIGN KEY (student_id)
            REFERENCES college.students(id)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM college.groups)
BEGIN
    INSERT INTO college.groups (id, name)
    VALUES
        (1, N'ИС-31'),
        (2, N'ИС-32'),
        (3, N'П-31');
END;
GO

IF NOT EXISTS (SELECT 1 FROM college.students)
BEGIN
    INSERT INTO college.students (id, name, group_id)
    VALUES
        (1, N'Алексей Смирнов', 1),
        (2, N'Иван Петров', 1),
        (3, N'Максим Волков', 1),
        (4, N'Анна Кузнецова', 1),
        (5, N'Дмитрий Орлов', 2),
        (6, N'Егор Соколов', 2),
        (7, N'Мария Попова', 2),
        (8, N'Ольга Морозова', 2),
        (9, N'Никита Фёдоров', 3),
        (10, N'Полина Васильева', 3);
END;
GO

IF NOT EXISTS (SELECT 1 FROM college.grades)
BEGIN
    INSERT INTO college.grades
        (id, student_id, subject, grade, grade_date)
    VALUES
        (1, 1, N'Базы данных', 4.50, '2026-09-01'),
        (2, 1, N'Базы данных', 5.00, '2026-09-08'),
        (3, 1, N'Базы данных', 4.00, '2026-09-15'),

        (4, 2, N'Базы данных', 4.00, '2026-09-01'),
        (5, 2, N'Базы данных', 4.50, '2026-09-08'),
        (6, 2, N'Базы данных', 4.50, '2026-09-15'),

        (7, 3, N'Базы данных', 3.50, '2026-09-01'),
        (8, 3, N'Базы данных', 4.00, '2026-09-08'),
        (9, 3, N'Базы данных', 4.50, '2026-09-15'),

        (10, 4, N'Базы данных', 5.00, '2026-09-01'),
        (11, 4, N'Базы данных', 5.00, '2026-09-08'),
        (12, 4, N'Базы данных', 4.50, '2026-09-15'),

        (13, 5, N'Базы данных', 4.50, '2026-09-01'),
        (14, 5, N'Базы данных', 4.00, '2026-09-08'),
        (15, 5, N'Базы данных', 5.00, '2026-09-15'),

        (16, 6, N'Базы данных', 3.50, '2026-09-01'),
        (17, 6, N'Базы данных', 4.00, '2026-09-08'),
        (18, 6, N'Базы данных', 4.00, '2026-09-15'),

        (19, 7, N'Базы данных', 5.00, '2026-09-01'),
        (20, 7, N'Базы данных', 4.50, '2026-09-08'),
        (21, 7, N'Базы данных', 5.00, '2026-09-15'),

        (22, 8, N'Базы данных', 4.00, '2026-09-01'),
        (23, 8, N'Базы данных', 4.00, '2026-09-08'),
        (24, 8, N'Базы данных', 4.50, '2026-09-15'),

        (25, 9, N'Базы данных', 3.50, '2026-09-01'),
        (26, 9, N'Базы данных', 4.00, '2026-09-08'),
        (27, 9, N'Базы данных', 4.00, '2026-09-15'),

        (28, 10, N'Базы данных', 4.50, '2026-09-01'),
        (29, 10, N'Базы данных', 5.00, '2026-09-08'),
        (30, 10, N'Базы данных', 5.00, '2026-09-15');
END;
GO

SELECT
    s.name,
    g.name AS group_name,
    gr.subject,
    gr.grade,
    gr.grade_date
FROM college.grades gr
JOIN college.students s
    ON gr.student_id = s.id
JOIN college.groups g
    ON s.group_id = g.id
ORDER BY s.name, gr.grade_date;
GO

SELECT
    s.id,
    s.name,
    g.name AS group_name,
    AVG(gr.grade) AS average_grade,
    MAX(gr.grade) AS max_grade,
    MIN(gr.grade) AS min_grade,
    COUNT(gr.id) AS grades_count
FROM college.students s
JOIN college.groups g
    ON s.group_id = g.id
JOIN college.grades gr
    ON gr.student_id = s.id
GROUP BY
    s.id,
    s.name,
    g.name
ORDER BY average_grade DESC;
GO

WITH StudentStats AS
(
    SELECT
        s.id,
        s.name,
        g.name AS group_name,
        AVG(gr.grade) AS average_grade
    FROM college.students s
    JOIN college.groups g
        ON s.group_id = g.id
    JOIN college.grades gr
        ON gr.student_id = s.id
    GROUP BY
        s.id,
        s.name,
        g.name
)
SELECT
    name,
    group_name,
    average_grade,
    ROW_NUMBER() OVER
    (
        ORDER BY average_grade DESC
    ) AS rating_position
FROM StudentStats
ORDER BY rating_position;
GO

WITH StudentStats AS
(
    SELECT
        s.id,
        s.name,
        g.name AS group_name,
        AVG(gr.grade) AS average_grade
    FROM college.students s
    JOIN college.groups g
        ON s.group_id = g.id
    JOIN college.grades gr
        ON gr.student_id = s.id
    GROUP BY
        s.id,
        s.name,
        g.name
)
SELECT
    name,
    group_name,
    average_grade,
    ROW_NUMBER() OVER
    (
        ORDER BY average_grade DESC
    ) AS row_number_rating,
    RANK() OVER
    (
        ORDER BY average_grade DESC
    ) AS rank_rating,
    DENSE_RANK() OVER
    (
        ORDER BY average_grade DESC
    ) AS dense_rank_rating
FROM StudentStats
ORDER BY average_grade DESC;
GO

WITH StudentStats AS
(
    SELECT
        s.id,
        s.name,
        g.name AS group_name,
        AVG(gr.grade) AS average_grade
    FROM college.students s
    JOIN college.groups g
        ON s.group_id = g.id
    JOIN college.grades gr
        ON gr.student_id = s.id
    GROUP BY
        s.id,
        s.name,
        g.name
)
SELECT
    name,
    group_name,
    average_grade,
    ROW_NUMBER() OVER
    (
        PARTITION BY group_name
        ORDER BY average_grade DESC
    ) AS group_position
FROM StudentStats
ORDER BY group_name, group_position;
GO

WITH StudentStats AS
(
    SELECT
        s.id,
        s.name,
        g.name AS group_name,
        AVG(gr.grade) AS average_grade
    FROM college.students s
    JOIN college.groups g
        ON s.group_id = g.id
    JOIN college.grades gr
        ON gr.student_id = s.id
    GROUP BY
        s.id,
        s.name,
        g.name
),
RankedStudents AS
(
    SELECT
        id,
        name,
        group_name,
        average_grade,
        ROW_NUMBER() OVER
        (
            PARTITION BY group_name
            ORDER BY average_grade DESC, id
        ) AS group_position
    FROM StudentStats
)
SELECT
    name,
    group_name,
    average_grade,
    group_position
FROM RankedStudents
WHERE group_position = 1
ORDER BY group_name;
GO

SELECT
    s.name,
    gr.grade_date,
    gr.grade,
    LAG(gr.grade) OVER
    (
        PARTITION BY gr.student_id
        ORDER BY gr.grade_date
    ) AS previous_grade
FROM college.grades gr
JOIN college.students s
    ON gr.student_id = s.id
ORDER BY
    s.name,
    gr.grade_date;
GO

WITH GradeChanges AS
(
    SELECT
        s.name,
        gr.grade_date,
        gr.grade,
        LAG(gr.grade) OVER
        (
            PARTITION BY gr.student_id
            ORDER BY gr.grade_date
        ) AS previous_grade
    FROM college.grades gr
    JOIN college.students s
        ON gr.student_id = s.id
)
SELECT
    name,
    grade_date,
    grade AS current_grade,
    previous_grade,
    grade - previous_grade AS grade_change,
    CASE
        WHEN previous_grade IS NULL
            THEN N'Нет предыдущего результата'
        WHEN grade > previous_grade
            THEN N'Улучшение'
        WHEN grade < previous_grade
            THEN N'Снижение'
        ELSE N'Без изменений'
    END AS status
FROM GradeChanges
ORDER BY name, grade_date;
GO

SELECT
    s.name,
    gr.grade_date,
    gr.grade AS current_grade,
    LEAD(gr.grade) OVER
    (
        PARTITION BY gr.student_id
        ORDER BY gr.grade_date
    ) AS next_grade
FROM college.grades gr
JOIN college.students s
    ON gr.student_id = s.id
ORDER BY
    s.name,
    gr.grade_date;
GO

SELECT
    s.name,
    gr.grade_date,
    gr.grade,
    AVG(gr.grade) OVER
    (
        PARTITION BY gr.student_id
    ) AS average_grade
FROM college.grades gr
JOIN college.students s
    ON gr.student_id = s.id
ORDER BY
    s.name,
    gr.grade_date;
GO

SELECT
    s.name,
    gr.grade_date,
    gr.grade,
    SUM(gr.grade) OVER
    (
        PARTITION BY gr.student_id
        ORDER BY gr.grade_date
        ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
    ) AS cumulative_grade
FROM college.grades gr
JOIN college.students s
    ON gr.student_id = s.id
ORDER BY
    s.name,
    gr.grade_date;
GO

SELECT
    s.name,
    g.name AS group_name,
    gr.grade_date,
    gr.grade AS current_grade,

    AVG(gr.grade) OVER
    (
        PARTITION BY gr.student_id
    ) AS average_grade,

    LAG(gr.grade) OVER
    (
        PARTITION BY gr.student_id
        ORDER BY gr.grade_date
    ) AS previous_grade,

    LEAD(gr.grade) OVER
    (
        PARTITION BY gr.student_id
        ORDER BY gr.grade_date
    ) AS next_grade,

    gr.grade -
    LAG(gr.grade) OVER
    (
        PARTITION BY gr.student_id
        ORDER BY gr.grade_date
    ) AS grade_change,

    SUM(gr.grade) OVER
    (
        PARTITION BY gr.student_id
        ORDER BY gr.grade_date
        ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
    ) AS cumulative_grade

FROM college.grades gr
JOIN college.students s
    ON gr.student_id = s.id
JOIN college.groups g
    ON s.group_id = g.id
ORDER BY
    s.name,
    gr.grade_date;
GO

WITH StudentStats AS
(
    SELECT
        s.id,
        s.name,
        g.name AS group_name,
        AVG(gr.grade) AS average_grade,
        MAX(gr.grade) AS max_grade,
        MIN(gr.grade) AS min_grade,
        COUNT(gr.id) AS grades_count,
        SUM(gr.grade) AS total_grade
    FROM college.students s
    JOIN college.groups g
        ON s.group_id = g.id
    JOIN college.grades gr
        ON gr.student_id = s.id
    GROUP BY
        s.id,
        s.name,
        g.name
)
SELECT
    name,
    group_name,
    average_grade,
    max_grade,
    min_grade,
    grades_count,
    total_grade,
    RANK() OVER
    (
        ORDER BY total_grade DESC
    ) AS rating
FROM StudentStats
ORDER BY rating, name;
GO

WITH GradeChanges AS
(
    SELECT
        s.name,
        gr.student_id,
        gr.grade_date,
        gr.grade,
        LAG(gr.grade) OVER
        (
            PARTITION BY gr.student_id
            ORDER BY gr.grade_date
        ) AS previous_grade
    FROM college.grades gr
    JOIN college.students s
        ON gr.student_id = s.id
)
SELECT
    name,
    grade_date,
    grade AS current_grade,
    previous_grade,
    grade - previous_grade AS grade_change,
    CASE
        WHEN previous_grade IS NULL
            THEN N'Нет предыдущего результата'
        WHEN grade > previous_grade
            THEN N'Улучшение'
        WHEN grade < previous_grade
            THEN N'Снижение'
        ELSE N'Без изменений'
    END AS status
FROM GradeChanges
ORDER BY name, grade_date;
GO

WITH StudentStats AS
(
    SELECT
        s.id,
        s.name,
        g.name AS group_name,
        AVG(gr.grade) AS average_grade,
        MAX(gr.grade) AS max_grade,
        MIN(gr.grade) AS min_grade,
        COUNT(gr.id) AS grades_count
    FROM college.students s
    JOIN college.groups g
        ON s.group_id = g.id
    JOIN college.grades gr
        ON gr.student_id = s.id
    GROUP BY
        s.id,
        s.name,
        g.name
),
FinalRating AS
(
    SELECT
        id,
        name,
        group_name,
        average_grade,
        max_grade,
        min_grade,
        grades_count,

        RANK() OVER
        (
            ORDER BY average_grade DESC
        ) AS overall_rating,

        ROW_NUMBER() OVER
        (
            PARTITION BY group_name
            ORDER BY average_grade DESC, id
        ) AS group_rating

    FROM StudentStats
)
SELECT
    name,
    group_name,
    average_grade,
    max_grade,
    min_grade,
    grades_count,
    overall_rating,
    group_rating
FROM FinalRating
ORDER BY overall_rating, name;
GO

WITH GradeAnalytics AS
(
    SELECT
        s.name,
        g.name AS group_name,
        gr.student_id,
        gr.grade_date,
        gr.grade AS current_grade,

        AVG(gr.grade) OVER
        (
            PARTITION BY gr.student_id
        ) AS average_grade,

        LAG(gr.grade) OVER
        (
            PARTITION BY gr.student_id
            ORDER BY gr.grade_date
        ) AS previous_grade,

        LEAD(gr.grade) OVER
        (
            PARTITION BY gr.student_id
            ORDER BY gr.grade_date
        ) AS next_grade,

        SUM(gr.grade) OVER
        (
            PARTITION BY gr.student_id
            ORDER BY gr.grade_date
            ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
        ) AS cumulative_grade

    FROM college.grades gr
    JOIN college.students s
        ON gr.student_id = s.id
    JOIN college.groups g
        ON s.group_id = g.id
)
SELECT
    name,
    group_name,
    grade_date,
    current_grade,
    average_grade,
    previous_grade,
    next_grade,
    current_grade - previous_grade AS grade_change,
    CASE
        WHEN previous_grade IS NULL
            THEN N'Нет предыдущего результата'
        WHEN current_grade > previous_grade
            THEN N'Улучшение'
        WHEN current_grade < previous_grade
            THEN N'Снижение'
        ELSE N'Без изменений'
    END AS status,
    cumulative_grade
FROM GradeAnalytics
ORDER BY
    name,
    grade_date;
GO
